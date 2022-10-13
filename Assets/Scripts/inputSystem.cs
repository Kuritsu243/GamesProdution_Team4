using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputSystem : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Vector2 _movementInput;
    public Vector2 mousePos;
    public float verticalInput;
    public float horizontalInput;
    public float mousePosX;
    public float mousePosY;
    private Camera mainCamera;
    public Vector3 worldPos;
    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.Move.performed += i => _movementInput = i.ReadValue<Vector2>();
            _playerControls.Player.Look.performed += i => mousePos = i.ReadValue<Vector2>();
            
        }
        
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCursorInput();
    }
    
    private void HandleMovementInput()
    {
        verticalInput = _movementInput.y;
        horizontalInput = _movementInput.x;
    }

    private void HandleCursorInput()
    {
        mousePosX = mousePos.x;
        mousePosY = mousePos.y;
    }
    
}
