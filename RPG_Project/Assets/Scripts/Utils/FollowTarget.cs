using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Common")]
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Vector3 _delta;
    [SerializeField]
    private bool isLookAt = false;

    [Header("Lerp")]
    [SerializeField]
    private bool isLerp = false;
    [SerializeField]
    private float _lerpSpeed = 0.0f;
    [SerializeField]
    private bool _isXLerp = false;
    [SerializeField]
    private bool _isYLerp = false;
    [SerializeField]
    private bool _isZLerp = false;

    private void Start()
    {
        if (_delta != Vector3.zero)
            transform.position = _delta;
    }

    private void LateUpdate()
    {
        if (isLerp)
        {
            Vector3 targetPos = GetLerpVector();

            if (targetPos == Vector3.zero)
                return;

            targetPos += _delta;

            transform.position = Vector3.Lerp(transform.position, targetPos, _lerpSpeed * Time.deltaTime);
        }
        else
            transform.position = _target.transform.position + _delta;

        if (isLookAt)
            transform.LookAt(_target.transform.position);
    }

    private Vector3 GetLerpVector()
    {
        Vector3 vec = Vector3.zero;

        if (_isXLerp)
            vec.x = _target.transform.position.x;
        if (_isYLerp)
            vec.y = _target.transform.position.y;
        if (_isZLerp)
            vec.z = _target.transform.position.z;

        return vec;
    }
}
