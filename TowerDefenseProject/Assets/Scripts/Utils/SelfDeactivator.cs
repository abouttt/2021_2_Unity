using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivator : MonoBehaviour
{
    [SerializeField]
    private float _deactiveTime = 0.0f;

    private void OnEnable()
    {
        StartCoroutine(PushPool());
    }

    private IEnumerator PushPool()
    {
        yield return new WaitForSeconds(_deactiveTime);

        ResourceManager.Destroy(gameObject);
    }
}
