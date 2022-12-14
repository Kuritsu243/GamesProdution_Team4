using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class inputSystem : MonoBehaviour
{
    [SerializeField] private bool _usingDesktopInput;
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
    private Camera _mainCamera;
    public Vector3 worldPos;
    private void OnEnable()
    {
        if (_playerControls == null)
        {
            // if (_usingDesktopInput)
            // {
            //     _playerControls = new PlayerControls();
            //     _playerControls.Player.Move.performed += i => _movementInput = i.ReadValue<Vector2>();
            //     _playerControls.Player.Fire.performed += i => ClickAction(i.ReadValue<float>());
            //     _playerControls.Player.Look
            // }
            
            _playerControls = new PlayerControls();
            _playerControls.Player.Move.performed += i => _movementInput = i.ReadValue<Vector2>();
            _playerControls.Player.Look.performed += i => mousePos = i.ReadValue<Vector2>();

            if (Application.platform is RuntimePlatform.Android or RuntimePlatform.WindowsEditor)
            {
                _playerControls.Player.Look.ChangeBinding(0).Erase();
                _playerControls.Player.Fire.performed += i => _rightJoyInput = i.ReadValue<Vector2>();
                // _playerControls.Player.Fire.performed += i => ClickAction(i.ReadValue<float>());
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
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        if (Application.platform is RuntimePlatform.WindowsEditor or RuntimePlatform.Android)
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
