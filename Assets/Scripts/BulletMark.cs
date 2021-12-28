using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviour
{
    [SerializeField]
    private float _returnSec = 3.0f;

    private ObjectPool _bulletMarkPool;

    private void OnEnable()
    {
        StartCoroutine("Return");
    }

    public void Setup(ObjectPool pool)
    {
        _bulletMarkPool = pool;

        
        StartCoroutine("Return");
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(_returnSec);
        gameObject.transform.parent = null;
        _bulletMarkPool.Return(gameObject);
    }
}
