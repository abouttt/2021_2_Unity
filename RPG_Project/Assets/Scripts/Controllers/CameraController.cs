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

    private RaycastHit[] _obstacleHits = null;
    private Color _colorTransparent = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    private Color _colorOrigin = new Color(1.0f, 1.0f, 1.0f, 1.0f);

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
            for (int i = 0; i < _obstacleHits.Length; i++)
            {
                Renderer renderer = _obstacleHits[i].collider.GetComponentInChildren<Renderer>();
                renderer.material.color = _colorOrigin;
            }
        }

        RaycastHit[] obstacleHits = Physics.RaycastAll(_target.transform.position, _delta, 100.0f, LayerMask.GetMask("Obstacle"));
        for (int i = 0; i < obstacleHits.Length; i++)
        {
            Renderer renderer = obstacleHits[i].collider.GetComponentInChildren<Renderer>();
            renderer.material.color = _colorTransparent;
        }

        _obstacleHits = obstacleHits;
    }
}
