using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
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
                if (!SwapItem(gameObject.transform.GetChild(0).gameObject, draggingItem))
                    return;

            draggingItem.transform.SetParent(transform);
            draggingItem.GetComponent<RectTransform>().position = _rect.position;
            draggingItem.GetComponent<UI_Item>().IsEquipment = false;
            IsHasItem = true;
        }
    }

    private bool SwapItem(GameObject inSlotItem, GameObject draggingItem)
    {
        ItemInfo inSlotItemItemInfo = inSlotItem.GetComponent<ItemInfo>();
        ItemInfo draggingItemItemInfo = draggingItem.GetComponent<ItemInfo>();

        UI_Item inSlotItem_UIItem = inSlotItem.GetComponent<UI_Item>();
        UI_Item draggingItem_UIItem = draggingItem.GetComponent<UI_Item>();

        // 수량이 있는 아이템인지 확인
        if (inSlotItemItemInfo.IsCountable &&
            draggingItemItemInfo.IsCountable &&
            inSlotItemItemInfo.Name == draggingItemItemInfo.Name)
        {
            inSlotItemItemInfo.Amount += draggingItemItemInfo.Amount;
            inSlotItem_UIItem.UpdateItemAction.Invoke();
            Managers.Resource.Destroy(draggingItem);
            UI_PopupCanvas.Instance.IsDragging = false;
            return false;
        }
        else
        {
            if (draggingItem_UIItem.IsEquipment)
            {
                if (inSlotItemItemInfo.Kinds == draggingItemItemInfo.Kinds)
                {
                    inSlotItem_UIItem.IsEquipment = true;
                    draggingItem_UIItem.IsEquipment = false;
                    draggingItem_UIItem.PrevParent.GetComponent<UI_EquipmentSlot>().IsHasItem = true;
                }
                else
                {
                    draggingItem.transform.SetParent(draggingItem_UIItem.PrevParent);
                    draggingItem.GetComponent<RectTransform>().position =
                        draggingItem_UIItem.PrevParent.GetComponent<RectTransform>().position;
                    return false;
                }
            }
            else
                draggingItem_UIItem.PrevParent.GetComponent<UI_ItemSlot>().IsHasItem = true;

            inSlotItem.transform.SetParent(draggingItem_UIItem.PrevParent);
            inSlotItem.GetComponent<RectTransform>().position = draggingItem_UIItem.PrevParent.GetComponent<RectTransform>().position;
        }

        return true;
    }
}
