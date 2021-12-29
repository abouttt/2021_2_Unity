using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Infomation")]
    [SerializeField]
    WeaponInformation  _weaponInformation;

    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private float      _explosionDelayTime;
    [SerializeField]
    private float      _explosionRadius;
    [SerializeField]
    private float      _explosionForce;

    private void OnEnable()
    {
        StartCoroutine("Explosion");
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(_explosionDelayTime);

        GetComponent<AudioSource>().Play();

        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(_explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "ExplosionDrum")
            {
                collider.transform.gameObject.GetComponent<ExplosionDrum>().TakeDamage(1000);
            }

            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
