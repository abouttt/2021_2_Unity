using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField]
    private ItemInfo _itemInfo = null;

    public ItemInfo ItemInfo => _itemInfo;
    public int Index { get; set; }
    public bool IsHasItem { get; set; } = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
