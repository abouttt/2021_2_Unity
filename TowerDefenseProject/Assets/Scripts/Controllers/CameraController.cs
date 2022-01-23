using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 30.0f;
    [SerializeField]
    private float _zoomSpeed = 400.0f;

    [SerializeField]
    private float _minX = -10.0f;
    [SerializeField]
    private float _maxX = 10.0f;

    [SerializeField]
    private float _minY = 4.0f;
    [SerializeField]
    private float _maxY = 25.0f;

    [SerializeField]
    private float _minZ = 4.0f;
    [SerializeField]
    private float _maxZ = 25.0f;

    [SerializeField]
    private Vector3 _originalViewPos;

    private void Update()
    {
        Move();
        Zoom();
        ResetPosition();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            h *= _moveSpeed * Time.deltaTime;
            v *= _moveSpeed * Time.deltaTime;

            Vector3 nextPos = transform.position + new Vector3(h, 0, v);
            if (nextPos.x < _minX)
                nextPos.x = _minX;
            if (nextPos.x > _maxX)
                nextPos.x = _maxX;
            if (nextPos.z < _minZ)
                nextPos.z = _minZ;
            if (nextPos.z > _maxZ)
                nextPos.z = _maxZ;

            transform.position = nextPos;
        }
    }

    private void Zoom()
    {
        Vector3 mouseWheel = Input.mouseScrollDelta;

        if (mouseWheel.y > 0)
        {
            float z = transform.localPosition.z;
            transform.Translate(Vector3.forward * _zoomSpeed * Time.deltaTime);

            if (transform.localPosition.y < _minY)
                transform.localPosition = new Vector3(transform.localPosition.x, _minY, z);
        }
        else if (mouseWheel.y < 0)
        {
            float z = transform.localPosition.z;
            transform.Translate(Vector3.back * _zoomSpeed * Time.deltaTime);

            if (transform.localPosition.y > _maxY)
                transform.localPosition = new Vector3(transform.localPosition.x, _maxY, z);
        }
    }

    private void ResetPosition()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position = _originalViewPos;
            transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        }
    }
}
