using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController2 : MonoBehaviour
{
    [SerializeField] private float cameraHeight;
    [SerializeField] private float cameraZOffset;
    [SerializeField] private int radiusStartSize;
    [SerializeField] private int radiusChangeRate;
    [SerializeField] private Vector3 radiusMaxSize;
    private inputSystem _inputSystem;
    private playerMovement _playerMovement;
    private cameraFollow _cameraFollow;
    private renderWithinRadius _playerLight;
    private GameObject _player;
    private Camera _playerCamera;
    // Start is called before the first frame update
    private void Start()
    {
        _player = gameObject;
        _inputSystem = GetComponent<inputSystem>();
        _playerMovement = GetComponent<playerMovement>();
        _playerLight = GetComponentInChildren<renderWithinRadius>();
        _cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraFollow>();
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // _playerLight.Init(radiusChangeRate, radiusStartSize, radiusMaxSize);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        _inputSystem.HandleAllInputs();
        _playerMovement.HandleAllMovement();
        _cameraFollow.MoveCamera(cameraHeight,cameraZOffset, _player.transform.position);
    }
}
