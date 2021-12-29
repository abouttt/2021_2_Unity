using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun,
    Grenade
}

[System.Serializable]
public struct WeaponInformation
{
    public string     Name;
    public WeaponType eWeaponType;
    public int        MaxAmmo;
    public int        CurrentAmmo;
    public int        MaxMagazine;
    public int        CurrentMagazine;
    public int        WeaponDamage;
}
