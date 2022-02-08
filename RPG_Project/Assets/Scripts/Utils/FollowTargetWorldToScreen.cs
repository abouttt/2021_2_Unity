using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetWorldToScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;
    [SerializeField]
    private Vector3 _delta;

    private void Update()
    {
        if (_target == null)
            return;

        transform.position = Camera.main.WorldToScreenPoint(_target.transform.position) + _delta;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
