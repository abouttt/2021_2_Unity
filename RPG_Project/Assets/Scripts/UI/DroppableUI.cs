using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image _image;
    private RectTransform _rect;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemInfo itemInfo = Util.FindChild<ItemInfo>(gameObject);
            if (itemInfo != null)
            {
                itemInfo.gameObject.transform.SetParent(eventData.pointerDrag.GetComponent<DraggableUI>().PrevParent);
                itemInfo.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DraggableUI>().PrevParent.GetComponent<RectTransform>().position;

                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = _rect.position;
            }
            else
            {
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = _rect.position;
            }
        }
    }

    private void SwapItem()
    {

    }
}
