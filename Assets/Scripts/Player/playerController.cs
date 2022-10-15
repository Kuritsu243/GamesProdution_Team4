using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Everytime you're outside a zone the light radius shrinks until you reach a safe zone - if light radius reaches zero then player dies
// TODO: the lower the health the lower the light of the player
// TODO: scale fuel with projectile charge
// TODO: Regain life from found candles / defeated moths






public class playerController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraHeight;
    [SerializeField] private float cameraRaycastLength;
    [SerializeField] private float cameraZOffset;
    
    [Header("Health Settings")] // Health Settings
    [SerializeField] private float burningRate;
    [SerializeField] private float maxPlayerHealth = 100;

    [Header("Light Radius Settings")] 
    [SerializeField] private Vector3 maxRadiusSize;
    [SerializeField] private int radiusSizeChangeRate;
    [SerializeField] private int radiusStartSize;

    [Header("Safe Zone Settings")] 
    [SerializeField] private int safeZoneRadius;
    
    [Header("Player Settings")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashFuelConsumption;
    
    [Header("Projectile Settings")] // Projectile Settings
    [SerializeField] private float playerMaxProjectileCharge;
    [SerializeField] private float playerProjectileChargeRate;
    [SerializeField] private float projectileHealthConsumption;
    [SerializeField] private float projectileDespawnRate;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileBaseDamage;
    [SerializeField] private int shootingRate;
    [SerializeField] private GameObject playerProjectile;

    [Header("World Settings")] 
    [SerializeField] private float worldGravity;
    //Private variables
    private int _maskingLayer;
    private bool _isAlive = true;
    private bool _isCharging;
    private bool _isReadyToFire = true;
    private bool _canDodge = true;
    private bool _currentlyDodging;
    private float _playerHealth;
    private float _projectileCalculatedDamage;
    private float _projectileCharge;
    private float _projectileChargeDuration = 0f;
    private float _projectileChargeStartTime;
    private Camera _playerCamera;
    private Color _lightRadiusColor;
    private cameraFollow _cameraFollowScript;
    private CharacterController _playerCharController;
    private GameObject _projectileSpawnPoint;
    private GameObject _spawnedObject;
    private GameObject _lightRadius;
    private Material _lightRadiusMaterial;
    private projectileScript _spawnedObjectScript;
    private renderWithinRadius _renderWithinRadiusScript;
    private Quaternion _playerNewRotation;
    private Rigidbody _playerRigidbody;
    private Vector3 _playerVelocity = Vector3.zero;
    private Vector3 _playerToMouse;
    private inputSystem _inputSystem;
    private playerMovement _playerMovement;

    // Encapsulated Fields
    public float PlayerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }

    public float MaxPlayerHealth
    {
        get => maxPlayerHealth;
        set => maxPlayerHealth = value;
    }

    public int SafeZoneRadius
    {
        get => safeZoneRadius;
        set => safeZoneRadius = value;
    }


    void Start()
    {
        // Find objects with tag
        // _projectileSpawnPoint = gameObject.FindGameObjectInChildWithTag("projectileSpawnPoint"); // find spawn point by tag in child
        // _lightRadius = gameObject.FindGameObjectInChildWithTag("lightRadius");
        //
        // // Get Components
        // _playerRigidbody = GetComponent<Rigidbody>();
        // _playerCharController = GetComponent<CharacterController>();
        // _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // _cameraFollowScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraFollow>();
        // _renderWithinRadiusScript = _lightRadius.GetComponent<renderWithinRadius>();
        // _lightRadiusMaterial = _lightRadius.GetComponent<Renderer>().material;
        // _maskingLayer = LayerMask.GetMask("Floor"); // Get Layer mask
        //
        // PlayerHealth = MaxPlayerHealth; // set player health to max health
        
        // set light radius alpha
        // _lightRadiusColor = _lightRadiusMaterial.color;
        // _lightRadiusColor.a = 0.025f;
        // get input system components
        _inputSystem = GetComponent<inputSystem>();
        _playerMovement = GetComponent<playerMovement>();
        
        
        // set render within radius variables
        // _renderWithinRadiusScript.MaxRadiusSize = maxRadiusSize;
        // _renderWithinRadiusScript.RadiusSizeChangeRate = radiusSizeChangeRate;
        // _renderWithinRadiusScript.RadiusStartSize = radiusStartSize;

    }
    
    
    
    private void Update()
    {
        _inputSystem.HandleAllInputs();
        _playerMovement.HandleAllMovement();
        // if (Input.GetMouseButtonDown(0) && _isReadyToFire && !_isCharging) // detect mouse 1 down
        // {
        //     _projectileChargeStartTime = Time.time; // stores value of the time when mouse 1 was down
        //     _isCharging = true;
        //     // calculates charge duration and clamps within set value of parameters
        // }
        // else if (Input.GetMouseButtonUp(0) && _isReadyToFire)
        // {
        //
        //     _projectileCalculatedDamage = projectileBaseDamage * _projectileChargeDuration; // calculates damage
        //     Shoot(_projectileCalculatedDamage); // shoots bullet
        //     StartCoroutine(ShootingCooldown());// start cooldown
        //     _isCharging = false;  
        // }
        // else if (_projectileChargeStartTime >= 0 && _isCharging)
        // {
        //     _projectileChargeDuration = Time.time - _projectileChargeStartTime;
        // }
    }

    private void FixedUpdate()
    {
        // var horizontal = Input.GetAxisRaw("Horizontal");
        // var vertical = Input.GetAxisRaw("Vertical");
        //
        // Move(horizontal, vertical);
        // Turn();

        //
        // _cameraFollowScript.MoveCamera(cameraHeight, cameraZOffset, transform.position);
        //
        // if (_isAlive)
        // {  // Decay health 
        //     _playerHealth = Mathf.Clamp(_playerHealth - (Time.deltaTime * burningRate), 0f, MaxPlayerHealth); 
        // }
        // if (_playerHealth  <= 0)
        // { // Player death
        //     _isAlive = false;
        //     this.Die();
        // }
    }


    // IEnumerator ShootingCooldown()
    // {
    //     _isReadyToFire = false; // player temporarily unable to fire
    //     yield return new WaitForSeconds(shootingRate); // cooldown 
    //     _isReadyToFire = true; // player can now fire
    //     _projectileChargeStartTime = 0f;
    //     _projectileChargeDuration = 0f;
    //
    // }

    // public void Shoot(float projectileDamage)
    // { // spawn object and assign it to a gameobject as reference
    //     // _projectileCharge = Mathf.Clamp(_projectileChargeDuration + projectileBaseDamage, projectileBaseDamage, playerMaxProjectileCharge); // min value is base damage, max value is max charge value
    //     // _spawnedObject = Instantiate(playerProjectile, _projectileSpawnPoint.transform.position, transform.rotation);
    //     // _spawnedObjectScript = _spawnedObject.GetComponent<projectileScript>(); // get projectile script of spawned object
    //     // _spawnedObjectScript.Init(projectileSpeed, projectileDamage, projectileDespawnRate, _projectileCharge); // pass through variables
    //     // Debug.Log("Spawned Bullet with a damage value of " + projectileDamage);
    //     // Damage(projectileHealthConsumption); // damage player
    // }
    

    // public void Move(float horizontal, float vertical)
    // {
    //     _playerVelocity.Set(horizontal, 0, vertical);
    //     _playerVelocity = _playerVelocity.normalized * (playerMovementSpeed * Time.deltaTime);
    //     // _playerCharController.Move(_playerVelocity);
    //     _playerRigidbody.MovePosition(_playerRigidbody.position + _playerVelocity);
    // }

    // public void Turn()
    // {
    //     var mouseLocation = _playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
    //
    //     if (Physics.Raycast(mouseLocation, out var floorHit, cameraRaycastLength, _maskingLayer))
    //     {
    //         _playerToMouse = floorHit.point - _playerRigidbody.position;
    //         _playerToMouse.y = 0;
    //         _playerNewRotation = Quaternion.LookRotation(_playerToMouse);
    //         _playerRigidbody.MoveRotation(_playerNewRotation);
    //     }
    // }

}

