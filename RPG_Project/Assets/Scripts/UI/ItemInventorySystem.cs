using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventorySystem : MonoBehaviour
{
    #region ÄÚ·çÆ¾
    static ItemInventorySystem s_instance;
    public static ItemInventorySystem Instance { get { return s_instance; } }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
    }
    #endregion

    [SerializeField]
    private KeyCode _activeKey = KeyCode.I;
    [SerializeField]
    private GameObject _slotsParent = null;

    private List<UI_ItemSlot> _slotList = null;

    private void Start()
    {
        Managers.Input.KeyAction += OpenInventory;

        SetItemSlot();
        CheckInventoryHasItem();
        gameObject.SetActive(false);
    }

    public void AddItem(ItemInfo itemInfo)
    {
        foreach (UI_ItemSlot slot in _slotList)
        {
            if (!slot.IsHasItem)
            {
                GameObject item = Managers.Resource.Instantiate("UI/ItemInventory/UI_Item", slot.transform);
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                item.GetComponent<ItemInfo>().CopyItemInfo(itemInfo);
                item.GetComponent<Image>().sprite = item.GetComponent<ItemInfo>().Icon;
                item.GetComponent<UI_Item>().UpdateItemAction.Invoke();
                slot.IsHasItem = true;
                break;
            }
        }
    }

    public void OnButtonCloseInventory()
    {
        gameObject.SetActive(false);
    }

    private void SetItemSlot()
    {
        UI_ItemSlot[] slots = _slotsParent.GetComponentsInChildren<UI_ItemSlot>();
        _slotList = new List<UI_ItemSlot>(slots.Length);
        for (int i = 0; i < slots.Length; i++)
        {
            _slotList.Add(slots[i]);
        }
    }

    private void CheckInventoryHasItem()
    {
        foreach (UI_ItemSlot slot in _slotList)
        {
            ItemInfo item = Util.FindChild<ItemInfo>(slot.gameObject);
            if (item != null)
            {
                slot.IsHasItem = true;
            }
        }
    }

    private void OpenInventory()
    {
        if (Input.GetKeyDown(_activeKey))
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
    }
}
