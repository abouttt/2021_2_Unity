using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityRotator : MonoBehaviour
{
    [SerializeField]
    private float _xRotSpeed = 0.0f;
    [SerializeField]
    private float _yRotSpeed = 0.0f;
    [SerializeField]
    private float _zRotSpeed = 0.0f;

    [SerializeField]
    private bool _isXRotate = false;
    [SerializeField]
    private bool _isYRotate = false;
    [SerializeField]
    private bool _isZRotate = false;

    private void Update()
    {
        if (_isXRotate)
            transform.Rotate(Vector3.right, _xRotSpeed * Time.deltaTime);
        if (_isYRotate)
            transform.Rotate(Vector3.up, _yRotSpeed * Time.deltaTime);
        if (_isZRotate)
            transform.Rotate(Vector3.forward, _zRotSpeed * Time.deltaTime);
    }
}
