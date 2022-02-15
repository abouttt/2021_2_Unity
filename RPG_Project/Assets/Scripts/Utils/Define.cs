using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum Layer
    {
        UI = 5,
        Ground,
        Obstacle,
        Monster,
        Item,
    }

    public enum ItemType
    {
        Immediate,  // 즉발
        Obtain,     // 얻기 가능
    }

    public enum ItemKinds
    {
        Helmet,
        Armor,
        Pants,
        Gloves,
        Shoes,
        Weapon,
        Portion,
    }

    public enum ItemTier
    {
        Normal,
        Rare,
        Legend,
    }
}
