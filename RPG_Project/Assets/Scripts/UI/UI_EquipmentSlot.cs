using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    [SerializeField]
    private Define.ItemKinds _slotKinds;

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

            // 아이템 종류가 다르다면 드래그한 아이템 제자리로 되돌리기
            if (draggingItem.GetComponent<ItemInfo>().Kinds != _slotKinds)
            {
                draggingItem.transform.SetParent(draggingItem.GetComponent<UI_Item>().PrevParent);
                draggingItem.GetComponent<RectTransform>().position = 
                    draggingItem.GetComponent<UI_Item>().PrevParent.GetComponent<RectTransform>().position;
                return;
            }

            if (IsHasItem)
                SwapItem(gameObject.transform.GetChild(1).gameObject, draggingItem);

            draggingItem.GetComponent<UI_Item>().IsEquipment = true;
            draggingItem.transform.SetParent(transform);
            draggingItem.GetComponent<RectTransform>().position = _rect.position;
            IsHasItem = true;
        }
    }

    private void SwapItem(GameObject inSlotItem, GameObject draggingItem)
    {
        UI_Item draggingItem_UIItem = draggingItem.GetComponent<UI_Item>();
        inSlotItem.transform.SetParent(draggingItem_UIItem.PrevParent);
        inSlotItem.GetComponent<RectTransform>().position = draggingItem_UIItem.PrevParent.GetComponent<RectTransform>().position;
        inSlotItem.GetComponent<UI_Item>().IsEquipment = false;
        draggingItem_UIItem.PrevParent.GetComponent<UI_ItemSlot>().IsHasItem = true;
    }
}
