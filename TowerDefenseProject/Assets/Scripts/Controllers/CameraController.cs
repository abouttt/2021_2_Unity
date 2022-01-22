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

        h *= _moveSpeed * Time.deltaTime;
        v *= _moveSpeed * Time.deltaTime;

        transform.position += new Vector3(h, 0, v);
    }

    private void Zoom()
    {
        Vector3 mouseWheel = Input.mouseScrollDelta;

        if (mouseWheel.y > 0)
        {
            transform.Translate(Vector3.forward * _zoomSpeed * Time.deltaTime);

            if (transform.localPosition.y < 2.0f)
                transform.localPosition = new Vector3(transform.localPosition.x, 2.0f, transform.localPosition.z);
        }
        else if (mouseWheel.y < 0)
        {
            transform.Translate(Vector3.back * _zoomSpeed * Time.deltaTime);

            if (transform.localPosition.y > 30.0f)
                transform.localPosition = new Vector3(transform.localPosition.x, 30.0f, transform.localPosition.z);
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
