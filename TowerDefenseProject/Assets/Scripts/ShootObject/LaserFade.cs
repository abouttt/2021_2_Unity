using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFade : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public Vector2 _uvAnimationRate = new Vector2(1.0f, 0.0f);
    private Vector2 _uvOffset = Vector2.zero;

    void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(Fade());
    }

    void Update()
    {
        _uvOffset += (_uvAnimationRate * Time.deltaTime);
        _lineRenderer.materials[0].SetTextureOffset("_MainTex", _uvOffset);
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, targetPos);
    }

    IEnumerator Fade()
    {
        float duration = 0;
        _lineRenderer.materials[0].SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
        while (duration < 1)
        {
            _lineRenderer.materials[0].SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, (1.0f - duration) * 0.5f));
            duration += Time.fixedDeltaTime * 4;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        _lineRenderer.materials[0].SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0));
    }
}
