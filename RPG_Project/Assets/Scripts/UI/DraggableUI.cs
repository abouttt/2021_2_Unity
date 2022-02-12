using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _canvas = null;
    private Transform _prevParent = null;
    private RectTransform _rect = null;
    private CanvasGroup _canvasGroup = null;

    public Transform PrevParent => _prevParent;

    private void Awake()
    {
        _canvas = FindObjectOfType<InventorySystem>().transform;
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _prevParent = transform.parent;

        transform.SetParent(_canvas);
        transform.SetAsLastSibling();

        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == _canvas)
        {
            transform.SetParent(_prevParent);
            _rect.position = _prevParent.GetComponent<RectTransform>().position;
        }

        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }
}
