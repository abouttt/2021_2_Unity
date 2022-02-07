using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayHitChangeColor : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private Color _color;

    private List<RaycastHit> _obstacleHits = new List<RaycastHit>();
    private readonly Color _colorOrigin = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private void LateUpdate()
    {
        RayHitObstacleTranslucency();
    }

    private void RayHitObstacleTranslucency()
    {
        if (_obstacleHits.Count > 0)
        {
            foreach (RaycastHit hit in _obstacleHits)
            {
                Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
                renderer.material.color = _colorOrigin;
            }
        }

        Vector3 dir = _target.transform.position - transform.position;
        Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir.normalized, dir.magnitude, _layerMask);
        _obstacleHits = hits.ToList();
        foreach (RaycastHit hit in _obstacleHits)
        {
            Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
            renderer.material.color = _color;
        }
    }
}
