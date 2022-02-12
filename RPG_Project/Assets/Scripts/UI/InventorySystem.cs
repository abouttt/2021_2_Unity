using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private KeyCode _activeKey = KeyCode.I;
    [SerializeField]
    private GameObject _slotsParent = null;
    [SerializeField]
    private GameObject _testItem = null;

    private List<UI_ItemSlot> _slotList = null;

    private void Start()
    {
        Managers.Input.KeyAction += OpenInventory;

        SetItemSlotIndex();
        CheckInventoryHasItem();
        AddItem(_testItem.GetComponent<ItemInfo>());
    }

    private void Update()
    {
        Debug.Log(_slotList.Capacity);
        Debug.Log(_slotList.Count);
    }

    public void AddItem(ItemInfo itemInfo)
    {
        foreach(UI_ItemSlot slot in _slotList)
        {
            if (!slot.IsHasItem)
            {
                GameObject item = Managers.Resource.Instantiate("Inventory/Item", slot.transform);
                item.GetComponent<ItemInfo>().CopyItemInfo(itemInfo);
                item.GetComponent<Image>().sprite = item.GetComponent<ItemInfo>().Icon;
                slot.IsHasItem = true;
                break;
            }
        }
    }

    private void SetItemSlotIndex()
    {
        UI_ItemSlot[] slots = _slotsParent.GetComponentsInChildren<UI_ItemSlot>();
        _slotList = new List<UI_ItemSlot>(slots.Length);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Index = i;
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

    public void OnButtonCloseInventory()
    {
        gameObject.SetActive(false);
    }
}
