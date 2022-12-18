using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed;
    private inputSystem _inputSystem;
    private Vector3 _moveDirection;
    private Vector2 _mousePos;
    private Vector3 _worldPos;
    private Quaternion _newRotation;
    private Quaternion _targetQuat;
    private Transform _mainCameraTransform;
    private Rigidbody _playerRigidbody;
    private Camera _mainCamera;
    private float _mousePosZ;
    private float _angle;
    private bool _isMoving;
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    private void Awake()
    {
        _inputSystem = GetComponent<inputSystem>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _mousePosZ = _mainCamera.farClipPlane * 0.5f;
        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     playerMovementSpeed *= 10;
        // }
    }

    public void HandleAllMovement()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        _moveDirection =  _mainCameraTransform.forward * _inputSystem.verticalInput;
        _moveDirection += _mainCameraTransform.right * _inputSystem.horizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0;
        _moveDirection *= playerMovementSpeed;

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.velocity = movementVelocity;
        _playerRigidbody.MovePosition(_playerRigidbody.position + _playerRigidbody.velocity);
        _isMoving = movementVelocity.magnitude > 0;
        
        // if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) && movementVelocity != Vector3.zero)
        // {
        //     transform.forward = movementVelocity;
        // }
    }
    private void HandleRotation()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (_inputSystem.rightJoyX != 0 || _inputSystem.rightJoyY != 0)
            {
                float angle = Mathf.Atan2(_inputSystem.rightJoyX, _inputSystem.rightJoyY) * Mathf.Rad2Deg;
                _playerRigidbody.MoveRotation(Quaternion.Euler(new Vector3(0, angle, 0)));
            }
        }
        else
        {
            _mousePos = _inputSystem.mousePos;
            if (!Physics.Raycast(_mainCamera.ScreenPointToRay(_inputSystem.mousePos), out RaycastHit hit)) return;
            _worldPos = hit.point - _playerRigidbody.position;
            _worldPos.y = 0f;
            _newRotation = Quaternion.LookRotation(_worldPos);
            _playerRigidbody.MoveRotation(_newRotation);
        }



    }
}
