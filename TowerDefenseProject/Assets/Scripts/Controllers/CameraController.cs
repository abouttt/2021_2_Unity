using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Define.CameraMode _mode = Define.CameraMode.OriginalView;

    [SerializeField]
    private Vector3 _originalViewPos;

    private void Start()
    {
        Managers.Input.MouseAction += OnMouseWheel;
        Managers.Input.MouseAction += OnMouseZoomIn;
        Managers.Input.MouseAction += OnMouseZoomOut;
    }

    private void Update()
    {
        if (_mode == Define.CameraMode.OriginalView)
            return;

        if (_mode == Define.CameraMode.FreeView)
        {

        }
    }

    public void SetOriginalView()
    {
        _mode = Define.CameraMode.OriginalView;
        transform.position = _originalViewPos;
    }

    public Vector3 GetScreenCenterPos()
    {
        return new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
    }

    public void OnMouseWheel(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.Wheel)
            return;

        SetOriginalView();
    }

    public void OnMouseZoomIn(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.ZoomIn)
            return;

        _mode = Define.CameraMode.FreeView;

        //Vector3 pos = GetScreenCenterPos().normalized;
        //transform.position += new Vector3(0.0f, 0.0f, 0.1f);
    }

    public void OnMouseZoomOut(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.ZoomOut)
            return;

        _mode = Define.CameraMode.FreeView;

        
    }
}
