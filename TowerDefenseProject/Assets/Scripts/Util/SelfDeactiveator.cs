using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SelfDeactiveator : MonoBehaviour
{
    [SerializeField]
    private string _poolName = null;
    [SerializeField]
    private float _returnTimeSec = 0.0f;

    public void OnEnable()
    {
        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(_returnTimeSec);

        EffectManager.Instance.HitSparkEftPool.Return(gameObject);
    }
}
