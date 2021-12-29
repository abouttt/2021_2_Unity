using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    [SerializeField]
    private float _returnSec = 3.0f;
    [SerializeField]
    private float _spin = 1.0f;

    private Rigidbody   _rigidBody;
    private AudioSource _audioSource;
    private ObjectPool  _bulletShellPool;

    private bool _isSoundPlay = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Setup(ObjectPool pool, Vector3 direction)
    {
        _bulletShellPool = pool;

        _rigidBody.velocity = new Vector3(direction.x, 1.0f, direction.z);
        _rigidBody.angularVelocity=new Vector3(Random.Range(-_spin, _spin),
                                               Random.Range(-_spin, _spin),
                                               Random.Range(-_spin, _spin));
        StartCoroutine("Return");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isSoundPlay)
        {
            _audioSource.Play();
            _isSoundPlay = true;
        }
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(_returnSec);
        _isSoundPlay = false;
        _bulletShellPool.Return(this.gameObject);
    }
}
