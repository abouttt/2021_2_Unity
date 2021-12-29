using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    static private PlayerInformation _instance;

    [SerializeField]
    private Transform  _weaponDefaultPosition;
    [SerializeField]
    private Transform  _weaponZoomPosition;
    private GameObject _equippedWeapon;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _equippedWeapon = _weaponDefaultPosition.GetChild(0).gameObject;
    }

    public static PlayerInformation Instnace
    {
        get => _instance;
    }

    public Transform WeaponDefaultPosition
    {
        get => _weaponDefaultPosition;
    }

    public Transform WeaponZoomPosition
    {
        get => _weaponZoomPosition;
    }

    public GameObject GetEquippedWeapon
    {
        get => _equippedWeapon;
    }
}
