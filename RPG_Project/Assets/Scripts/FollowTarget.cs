using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;  

    private void LateUpdate()
    {
        transform.position = _target.transform.position;
    }
}
