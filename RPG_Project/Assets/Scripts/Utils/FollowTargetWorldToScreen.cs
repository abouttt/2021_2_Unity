using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetWorldToScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;
    [SerializeField]
    private Vector3 _delta;

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 pos = Camera.main.WorldToScreenPoint(_target.transform.position + _delta);
        pos.z = 0;
        transform.position = pos;
    }

    public void SetTarget(GameObject target, Vector3 delta)
    {
        _target = target;
        _delta = delta;
    }
}
