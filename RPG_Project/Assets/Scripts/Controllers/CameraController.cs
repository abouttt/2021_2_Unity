using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;

    [SerializeField]
    private Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);

    private List<RaycastHit> _obstacleHits = new List<RaycastHit>();
    private readonly Color _colorTransparent = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    private readonly Color _colorOrigin = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private void Start()
    {
        if (_target == null)
            _target = GameObject.FindWithTag("Player");
    }

    private void LateUpdate()
    {
        FollowTarget();
        RayHitObstacleTranslucency();
    }

    private void FollowTarget()
    {
        transform.position = _target.transform.position + _delta;
        transform.LookAt(_target.transform);
    }

    private void RayHitObstacleTranslucency()
    {
        if (_obstacleHits != null)
        {
            foreach (RaycastHit hit in _obstacleHits)
            {
                Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
                renderer.material.color = _colorOrigin;
            }
        }

        float dist = Vector3.Distance(_target.transform.position, transform.position);
        _obstacleHits = new List<RaycastHit>(Physics.RaycastAll(_target.transform.position, _delta.normalized, dist, LayerMask.GetMask("Obstacle")));
        foreach (RaycastHit hit in _obstacleHits)
        {
            Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
            renderer.material.color = _colorTransparent;
        }
    }
}
