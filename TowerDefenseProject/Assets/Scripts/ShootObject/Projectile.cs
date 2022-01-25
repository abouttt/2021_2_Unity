using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    [SerializeField]
    private int _damage = 10;

    private Transform _target = null;

    private void Update()
    {
        if (_target == null)
        {
            ResourceManager.Instance.Destroy(gameObject);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void Setup(Vector3 pos, GameObject target)
    {
        transform.position = pos;
        _target = target.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target.gameObject)
        {
            _target = null;

            GameObject eft = ResourceManager.Instance.Instantiate("Effects/HitExplosion");
            eft.transform.position = transform.position;
            eft.transform.parent = other.transform;
            other.GetComponent<CreepController>().Hp -= _damage;
            ResourceManager.Instance.Destroy(gameObject);
        }
    }
}
