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
    private Vector2 _rightJoyInput;
    public float verticalInput;
    public float horizontalInput;
    public float mousePosX;
    public float mousePosY;
    public float rightJoyX;
    public float rightJoyY;
    private bool _mouseFire;
    public bool mouseFire;

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.Move.performed += i => _movementInput = i.ReadValue<Vector2>();
            _playerControls.Player.Look.performed += i => mousePos = i.ReadValue<Vector2>();

            if (Application.platform is RuntimePlatform.Android or RuntimePlatform.WindowsEditor or RuntimePlatform.WindowsPlayer)
            {
                _playerControls.Player.Look.ChangeBinding(0).Erase();
                _playerControls.Player.Fire.performed += i => _rightJoyInput = i.ReadValue<Vector2>();
            }
        }
        
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;

    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        if (Application.platform is RuntimePlatform.WindowsEditor or RuntimePlatform.Android or RuntimePlatform.WindowsPlayer)
        {
            HandleJoyInput();
        }
        else
        {
            HandleCursorInput();
        }

    }
    
    private void HandleMovementInput()
    {
        verticalInput = _movementInput.y;
        horizontalInput = _movementInput.x;
    }

    // private void Update()
    // {
    //     Debug.Log(mouseFire);
    //
    // }

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

    private void HandleJoyInput()
    {
        if (_rightJoyInput != Vector2.zero)
        {
            _mouseFire = true;
            mouseFire = true;
        }
        else
        {
            _mouseFire = false;
            mouseFire = false;
        }

        rightJoyX = _rightJoyInput.x;
        rightJoyY = _rightJoyInput.y;
    }
}
