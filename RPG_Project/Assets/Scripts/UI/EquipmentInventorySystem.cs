using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventorySystem : MonoBehaviour
{
    #region ÄÚ·çÆ¾
    static EquipmentInventorySystem s_instance;
    public static EquipmentInventorySystem Instance { get { return s_instance; } }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
    }
    #endregion

    [SerializeField]
    private KeyCode _activeKey = KeyCode.E;

    private void Start()
    {
        Managers.Input.KeyAction += OpenInventory;

        gameObject.SetActive(false);
    }

    public void OnButtonCloseInventory()
    {
        gameObject.SetActive(false);
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
