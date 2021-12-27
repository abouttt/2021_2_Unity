using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private float _rotXSpeed;
    [SerializeField]
    private float _rotYSpeed;

    private float _mouseX;
    private float _mouseY;
    private float _eulerAngleX;
    private float _eulerAngleY;

    private readonly float _minX = -80;
    private readonly float _maxX = 50;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        _eulerAngleX += _mouseY * _rotXSpeed;
        _eulerAngleY += _mouseX * _rotYSpeed;

        _eulerAngleX = Mathf.Clamp(_eulerAngleX, _minX, _maxX);

        transform.rotation = Quaternion.Euler(-_eulerAngleX, _eulerAngleY, 0);
    }
}
