using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _rotationSpeed = 20.0f;

    private Animator _animator = null;

    private Vector3 _destPos;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateMoving();
    }

    private void UpdateMoving()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
            {
                _destPos = hit.point;
            }
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            _animator.SetBool("isMove", false);
        }
        else
        {
            Debug.DrawRay(transform.position, dir.normalized * 0.5f, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 0.5f, LayerMask.GetMask("Obstacle")))
            {
                _animator.SetBool("isMove", false);
                return;
            }

            _animator.SetBool("isMove", true);
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);
        }
    }
}
