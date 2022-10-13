using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed;
    [SerializeField] private float rotateSmoothing;
    private inputSystem _inputSystem;
    private Vector3 _moveDirection;
    private Vector2 _mousePos;
    private Vector3 _worldPos;
    private Vector3 _targetDirection;
    private Quaternion _newRotation;
    private Quaternion _targetQuat;
    private Transform _mainCameraTransform;
    private Rigidbody _playerRigidbody;
    private Camera _mainCamera;
    private float _mousePosZ;
    private float _angle;

    private void Awake()
    {
        _inputSystem = GetComponent<inputSystem>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _mousePosZ = _mainCamera.farClipPlane * 0.5f;
    }

    public void HandleAllMovement()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        _moveDirection =  _mainCameraTransform.forward *_inputSystem.verticalInput;
        _moveDirection = _moveDirection + _mainCameraTransform.right * _inputSystem.horizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0;
        _moveDirection = _moveDirection * playerMovementSpeed;

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.velocity = movementVelocity;
        _playerRigidbody.MovePosition(_playerRigidbody.position + _playerRigidbody.velocity);
    }
    private void HandleRotation()
    {
        _mousePos = _inputSystem.mousePos;
        if (_mousePos.x != 0f && _mousePos.y != 0f)
        {
            _worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(_mousePos.x, 0f, _mousePos.y));
            _targetDirection = _worldPos - transform.position;
            // _newRotation = Quaternion.LookRotation(_targetDirection);
            // _playerRigidbody.MoveRotation(_newRotation);
            _angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
            _targetQuat = Quaternion.Euler((new Vector3(0f, -_angle + 90, 0f)));
            _playerRigidbody.MoveRotation(_targetQuat);
        }

    }
}
