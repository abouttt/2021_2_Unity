using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _rotationSpeed = 20.0f;

    private GameObject _clickMoveArrwow = null;

    private Animator _animator = null;
    private CharacterController _characterController = null;

    private Vector3 _destPos;
    private bool _isCanMove = false;

    private void Start()
    {
        InitComponent();
        InitClickMoveArrow();
    }

    private void Update()
    {
        CheckMoveable();
        SetDestinationPos();
        UpdateMoving();
    }

    private void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f || !_isCanMove)
        {
            _animator.SetBool("isMove", false);
            return;
        }

        _animator.SetBool("isMove", true);
        _characterController.Move(dir.normalized * _moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);
    }

    private void SetDestinationPos()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
            {
                _destPos = hit.point;

                SetClickMoveArrowPos(hit.point);
                _isCanMove = true;
            }
        }
    }

    private void SetClickMoveArrowPos(Vector3 pos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos.y = 0.1f;
            _clickMoveArrwow.transform.position = pos;
            _clickMoveArrwow.GetComponent<ParticleSystem>().Play();
        }
    }

    private void CheckMoveable()
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides)
            _isCanMove = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Teleport"))
            _isCanMove = false;
    }

    private void InitComponent()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void InitClickMoveArrow()
    {
        _clickMoveArrwow = Resources.Load<GameObject>("Prefabs/Interactive/ClickMoveArrows");
        _clickMoveArrwow = Instantiate(_clickMoveArrwow);
        _clickMoveArrwow.GetComponent<ParticleSystem>().Stop();
    }
}
