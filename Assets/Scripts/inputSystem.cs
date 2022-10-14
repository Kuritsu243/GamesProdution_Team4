using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class inputSystem : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Vector2 _movementInput;
    public Vector2 mousePos;
    public float verticalInput;
    public float horizontalInput;
    public float mousePosX;
    public float mousePosY;
    private bool _mouseFire;
    public bool mouseFire;
    private Camera mainCamera;
    public Vector3 worldPos;
    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.Move.performed += i => _movementInput = i.ReadValue<Vector2>();
            _playerControls.Player.Look.performed += i => mousePos = i.ReadValue<Vector2>();
            _playerControls.Player.Fire.performed += i => ClickAction(i.ReadValue<float>());


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

    private void Update()
    {
        Debug.Log(mouseFire);

    }

    private void HandleCursorInput()
    {
        mousePosX = mousePos.x;
        mousePosY = mousePos.y;
        mouseFire = _mouseFire;
    }

    private void ClickAction(float b)
    {
        _mouseFire = System.Convert.ToBoolean(b);
    }
}
