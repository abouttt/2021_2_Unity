using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _rotationSpeed = 20.0f;

    private Animator _animator = null;
    private CharacterController _characterController = null;

    private GameObject _target = null;
    private GameObject _clickMoveArrow = null;

    private Vector3 _destPos;
    private bool _isCanMove = true;

    private void Start()
    {
        Managers.Input.MouseAction += OnMouseEvent;

        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _clickMoveArrow = Managers.Resource.Instantiate("Interactive/ClickMoveArrows");
        _clickMoveArrow.GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {
        MoveToDestination();
        CheckMoveable();
    }

    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (evt == Define.MouseEvent.PointerUp ||
            UI_PopupCanvas.Instance.IsDragging)
            return;

        if (evt == Define.MouseEvent.PointerDown)
            SetTarget();

        SetDestination();

        if (evt == Define.MouseEvent.PointerDown)
            SetClickMoveArrowPos(_destPos);
    }

    private void CheckMoveable()
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides)
            _isCanMove = false;
    }

    private void SetTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, Util.GetLayerMask(Define.Layer.Ground, Define.Layer.Monster, Define.Layer.Item)))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                _target = null;
            else
            {
                _target = hit.collider.gameObject;
                _destPos = _target.gameObject.transform.position;
                _isCanMove = true;
            }
        }
    }

    private void SetDestination()
    {
        if (_target != null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, Util.GetLayerMask(Define.Layer.Ground)))
        {
            _destPos = hit.point;
            _isCanMove = true;
        }
    }

    private void MoveToDestination()
    {
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f || !_isCanMove)
        {
            if (_target != null)
            {
                if (_target.layer == (int)Define.Layer.Item && 
                    _target.GetComponent<ItemInfo>().Type == Define.ItemType.Obtain)
                {
                    ItemInventorySystem.Instance.AddItem(_target.GetComponent<ItemInfo>());
                    _target.GetComponent<FieldItem>().Destroy();
                }

                _target = null;
            }

            _animator.SetBool("isMove", false);
            return;
        }

        _animator.SetBool("isMove", true);
        _characterController.Move(dir.normalized * _moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);
    }

    private void SetClickMoveArrowPos(Vector3 pos)
    {
        if (_target != null)
            return;

        pos.y = 0.1f;
        _clickMoveArrow.transform.position = pos;
        _clickMoveArrow.GetComponent<ParticleSystem>().Play();
    }
}
