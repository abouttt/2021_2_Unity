using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private KeyCode _activeKey = KeyCode.I;

    private GraphicRaycaster _gr = null;
    private PointerEventData _ped = null;
    private List<RaycastResult> _raycastResultList = null;

    private void Start()
    {
        Managers.Input.KeyAction += OpenInventory;
        _gr = GetComponent<GraphicRaycaster>();
        _ped = new PointerEventData(null);
        _raycastResultList = new List<RaycastResult>();
    }

    private void Update()
    {
        _ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        _gr.Raycast(_ped, results);
        if (results.Count <= 0) 
            return;

        // 이벤트 처리부분
        //results[0].gameObject.transform.position = _ped.position;

    }

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _raycastResultList.Clear();

        _gr.Raycast(_ped, _raycastResultList);

        if (_raycastResultList.Count == 0)
            return null;

        return _raycastResultList[0].gameObject.GetComponent<T>();
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
