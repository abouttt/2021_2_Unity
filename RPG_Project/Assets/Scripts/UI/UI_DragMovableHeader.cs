using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DragMovableHeader : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private Transform _target;

    private Vector2 _beginPoint;
    private Vector2 _moveBegin;

    private bool _isCanDarg = false;

    private void Awake()
    {
        if (_target == null)
            _target = transform.parent;
    }

    private void OnDisable()
    {
        _isCanDarg = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isCanDarg = true;

        _beginPoint = _target.position;
        _moveBegin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isCanDarg)
            _target.position = _beginPoint + (eventData.position - _moveBegin);
    }
}
