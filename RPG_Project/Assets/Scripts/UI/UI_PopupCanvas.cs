using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_PopupCanvas : MonoBehaviour
{
    #region 코루틴
    static UI_PopupCanvas s_instance;
    public static UI_PopupCanvas Instance { get { return s_instance; } }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
    }
    #endregion

    private GraphicRaycaster _gr;
    private PointerEventData _ped;

    public bool IsDragging { get; set; } = false;

    private void Start()
    {
        _gr = GetComponent<GraphicRaycaster>();
        _ped = new PointerEventData(null);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            _gr.Raycast(_ped, results);

            if (results.Count > 0)
            {
                // 가장 위로 보이게 하기
            }
        }
    }
}
