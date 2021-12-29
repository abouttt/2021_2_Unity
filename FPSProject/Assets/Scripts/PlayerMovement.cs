using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float               _walkSpeed;
    [SerializeField]
    private float               _runSpeed;
    [SerializeField]
    private float               _jumpForce;

    private float               _moveSpeed;
    private AudioSource         _audioSource;

    private float               _moveX;
    private float               _moveZ;
    private Vector3             _moveVector;
    private Vector3             _rotateDirection;
    private Vector3             _moveDirection;

    private bool                _isRun;
    private readonly float      _gravity = -9.81f;

    [SerializeField]
    private Transform           _cameraTransform;
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _isRun = Input.GetKey(KeyCode.LeftShift);
        _moveSpeed = !_isRun ? _walkSpeed : _runSpeed;

        Move();
        Jump();
        GravitationalAction();
    }

    private void Move()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveZ = Input.GetAxisRaw("Vertical");

        _moveVector = new Vector3(_moveX, 0, _moveZ);
        _rotateDirection = _cameraTransform.rotation * _moveVector;
        _moveDirection = new Vector3(_rotateDirection.x, _moveDirection.y, _rotateDirection.z);

        _characterController.Move(_moveDirection * (_moveSpeed * Time.deltaTime));

        if (_moveX != 0 || _moveZ != 0)
        {
            if (!_audioSource.isPlaying && _characterController.isGrounded)
            {
                if (!_isRun)
                {
                    _audioSource.pitch = 0.7f;
                    _audioSource.Play();
                }
                else
                {
                    _audioSource.pitch = 1;
                    _audioSource.Play();
                }
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_characterController.isGrounded)
            {
                _moveDirection.y = _jumpForce;
            }
        }
    }

    private void GravitationalAction()
    {
        if (!_characterController.isGrounded)
        {
            _moveDirection.y += _gravity * Time.deltaTime;
        }
    }
}
