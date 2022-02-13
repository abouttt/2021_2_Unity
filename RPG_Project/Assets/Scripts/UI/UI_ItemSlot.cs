using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public int Index { get; set; }
    public bool IsHasItem { get; set; } = false;

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
        if (IsHasItem)
            Debug.Log("IsHavItem");
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
            GameObject slectItem = eventData.pointerDrag;

            if (itemInfo != null)
            {
                itemInfo.gameObject.transform.SetParent(slectItem.GetComponent<UI_Item>().PrevParent);
                itemInfo.GetComponent<RectTransform>().position = slectItem.GetComponent<UI_Item>().PrevParent.GetComponent<RectTransform>().position;
            }
            else
            {
                slectItem.GetComponent<UI_Item>().PrevParent.GetComponent<UI_ItemSlot>().IsHasItem = false;
            }

            slectItem.transform.SetParent(transform);
            slectItem.GetComponent<RectTransform>().position = _rect.position;

            IsHasItem = true;
        }
    }
}
