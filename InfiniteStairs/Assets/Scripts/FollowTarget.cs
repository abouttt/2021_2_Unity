using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform _targetPosition;
    [SerializeField]
    private float     _followSpeed;
    private Vector3   _nextPosition;

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        _nextPosition = new Vector3(_targetPosition.position.x, _targetPosition.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _nextPosition, _followSpeed * Time.deltaTime);
    }
}
