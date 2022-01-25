using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArrowController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 10.0f;

    private Vector3 _startPoint;
    private int _wayIndex = 0;

    private void Start()
    {
        _startPoint = SpawnManager.Instance.StartPoint + new Vector3(0.0f, 0.1f, 0.0f);
        transform.position = _startPoint;
    }

    private void Update()
    {
        MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
        Vector3 dir = SpawnManager.Instance.WayPoints[_wayIndex].position - transform.position;
        dir.y = 0.0f;
        transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir);

        if (dir.magnitude <= 0.1f)
        {
            _wayIndex++;
            if (SpawnManager.Instance.WayPoints.Count <= _wayIndex)
            {
                _wayIndex = 0;
                transform.position = _startPoint;
            }
        }
    }
}
