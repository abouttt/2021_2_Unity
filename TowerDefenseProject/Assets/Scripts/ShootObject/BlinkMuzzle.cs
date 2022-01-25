using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkMuzzle : MonoBehaviour
{
    private readonly float _blinkIntervalTime = 0.05f;
    private Renderer _render = null;

    private void Start()
    {
        _render = transform.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        if (_render != null)
            _render.enabled = true;
        StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            while (_render == null)
                yield return null;

            _render.enabled = !_render.enabled;
            yield return new WaitForSeconds(_blinkIntervalTime);
        }
    }
}
