using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivator : MonoBehaviour
{
    [SerializeField]
    private float _deactiveTime = 0.0f;

    private void OnEnable()
    {
        StartCoroutine(Deactivator());
    }

    private IEnumerator Deactivator()
    {
        yield return new WaitForSeconds(_deactiveTime);

        ResourceManager.Instance.Destroy(gameObject);
    }
}
