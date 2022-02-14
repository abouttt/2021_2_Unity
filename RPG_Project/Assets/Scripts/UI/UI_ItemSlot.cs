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
            GameObject draggingItem = eventData.pointerDrag;

            if (IsHasItem)
                SwapItem(gameObject.transform.GetChild(0).gameObject, draggingItem);

            draggingItem.transform.SetParent(transform);
            draggingItem.GetComponent<RectTransform>().position = _rect.position;
            IsHasItem = true;
        }
    }

    private void SwapItem(GameObject rhs, GameObject lhs)
    {
        ItemInfo rhsItemInfo = rhs.GetComponent<ItemInfo>();
        ItemInfo lhsItemInfo = lhs.GetComponent<ItemInfo>();

        if (rhsItemInfo.IsCountable && lhsItemInfo.IsCountable && 
            rhsItemInfo.Name == lhsItemInfo.Name)
        {
            rhsItemInfo.Amount += lhsItemInfo.Amount;
            rhs.GetComponent<UI_Item>().UpdateItemAction.Invoke();
            Managers.Resource.Destroy(lhs);
            InventorySystem.Instance.IsDragging = false;
        }
        else
        {
            UI_Item lhsUIItem = lhs.GetComponent<UI_Item>();
            rhs.transform.SetParent(lhsUIItem.PrevParent);
            rhs.GetComponent<RectTransform>().position = lhsUIItem.PrevParent.GetComponent<RectTransform>().position;
            lhsUIItem.PrevParent.GetComponent<UI_ItemSlot>().IsHasItem = true;
        }
    }
}
