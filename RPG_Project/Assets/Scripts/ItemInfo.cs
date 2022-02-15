using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private string _name = null;
    [SerializeField]
    private Sprite _iconSprite = null;
    [SerializeField]
    private Define.ItemKinds _kinds;
    [SerializeField]
    private Define.ItemType _type;
    [SerializeField]
    private Define.ItemTier _tier;
    [SerializeField]
    private bool _isCountable = false;
    [SerializeField]
    private int _amount;

    public string Name { get { return _name; } }
    public Sprite Icon { get { return _iconSprite; } }
    public Define.ItemKinds Kinds { get { return _kinds; } }
    public Define.ItemTier Tier { get { return _tier; } }
    public Define.ItemType Type { get { return _type; } }
    public bool IsCountable { get { return _isCountable; } }
    public int Amount { get { return _amount; } set { _amount = value; } }

    public void CopyItemInfo(ItemInfo itemInfo)
    {
        _name = itemInfo.Name;
        _iconSprite = itemInfo.Icon;
        _kinds = itemInfo.Kinds;
        _type = itemInfo.Type;
        _tier = itemInfo.Tier;
        _isCountable = itemInfo.IsCountable;
        _amount = itemInfo.Amount;
    }
}
