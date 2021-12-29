using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Gun              _gun;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI  _textWeaponName;

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI  _textAmmo;

    [Header("Magazine")]
    [SerializeField]
    private GameObject       _magazineUIPrefab;
    [SerializeField]
    private Transform        _magazineParent;

    private List<GameObject> _magazineList;


    private void Awake()
    {

        SetupWeapon();
        SetupMagazine();
        _gun._onAmmoEvent.AddListener(UpdateAmmoHUD);
        _gun._onMagazinEvent.AddListener(UpdateMagazineHUD);
    }

    private void SetupWeapon()
    {
        _textWeaponName.text = _gun.WeaponName;
    }

    private void SetupMagazine()
    {
        _magazineList = new List<GameObject>();
        for (int i = 0; i < _gun.MaxMagazine; i++)
        {
            GameObject clone = Instantiate(_magazineUIPrefab);
            clone.transform.SetParent(_magazineParent);
            clone.SetActive(false);
            _magazineList.Add(clone);
        }

        for (int i = 0; i < _gun.CurrentMagazine; i++)
        {
            _magazineList[i].SetActive(true);
        }
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        _textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        foreach (GameObject magazine in _magazineList)
        {
            magazine.SetActive(false);
        }
        for (int i = 0; i < currentMagazine; i++)
        {
            _magazineList[i].SetActive(true);
        }
    }
}
