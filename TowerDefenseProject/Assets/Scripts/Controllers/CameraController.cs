using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 _originalViewPos;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            transform.position = _originalViewPos;
        }
    }

}
