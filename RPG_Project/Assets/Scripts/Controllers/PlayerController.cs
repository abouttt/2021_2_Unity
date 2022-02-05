using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _rotationSpeed = 20.0f;
    [SerializeField]
    private GameObject _clickArrwowObject = null;

    private Animator _animator = null;
    private CharacterController _characterController = null;

    private Vector3 _destPos;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _clickArrwowObject = Instantiate(_clickArrwowObject);
        _clickArrwowObject.GetComponent<ParticleSystem>().Stop();
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
                
                SetClickArrowPos(hit.point);
            }
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            return;
        }
        else
        {
            _characterController.Move(dir.normalized * _moveSpeed * Time.deltaTime);
        }
    }

    //private void UpdateMoving()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
    //        {
    //            _destPos = hit.point;

    //            SetClickArrowPos(hit.point);
    //        }
    //    }

    //    Vector3 dir = _destPos - transform.position;
    //    dir.y = 0;

    //    if (dir.magnitude < 0.1f)
    //    {
    //        _animator.SetBool("isMove", false);
    //    }
    //    else
    //    {
    //        _animator.SetBool("isMove", true);

    //        Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized * 0.5f, Color.red);

    //        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 0.5f, LayerMask.GetMask("Obstacle", "Monster")))
    //            _destPos = transform.position;
    //        else
    //            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;

    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);
    //    }
    //}

    private void SetClickArrowPos(Vector3 pos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos.y = 0.1f;
            _clickArrwowObject.transform.position = pos;
            _clickArrwowObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
