using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.0f;

    private Transform _target = null;
    public Transform Target { set { _target = value; } }

    private void Start()
    {

    }

    private void Update()
    {
        if (_target == null)
            return;
        Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
        // transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target.gameObject)
        {
            _target = null;
            ResourceManager.Destroy(gameObject);
        }
    }
}
