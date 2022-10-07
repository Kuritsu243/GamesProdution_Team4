using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Health Settings")] // Health Settings
    [SerializeField] private float burningRate;
    [SerializeField] private float maxPlayerHealth = 100;
    
    [Header("Projectile Settings")] // Projectile Settings
    [SerializeField] private float playerMaxProjectileCharge;
    [SerializeField] private float playerProjectileChargeRate;
    [SerializeField] private float projectileHealthConsumption;
    [SerializeField] private float projectileDespawnRate;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileBaseDamage;
    [SerializeField] private int shootingRate;
    [SerializeField] private GameObject playerProjectile;
    
    //Private variables
    private float _playerHealth;
    private float _projectileCharge;
    private float _projectileCalculatedDamage;
    private float _projectileChargeDuration = 0f;
    private float _projectileChargeStartTime;
    private bool _isAlive = true;
    private bool _isReadyToFire = true;
    private GameObject _spawnedObject;
    private projectileScript _spawnedObjectScript;
    
    // Encapsulated Fields
    public float PlayerHealth
    {
        get => _playerHealth;
        private set => _playerHealth = value;
    }
    public float MaxPlayerHealth
    {
        get => maxPlayerHealth;
        set => maxPlayerHealth = value;
    }

    
    void Start()
    {
        PlayerHealth = MaxPlayerHealth; // set player health to max health
    }
    private void FixedUpdate()
    {
        if (_isAlive)
        {  // Decay health 
            _playerHealth = Mathf.Clamp(_playerHealth - (Time.deltaTime * burningRate), 0f, MaxPlayerHealth); 
        }
        if (_playerHealth  <= 0)
        { // Player death
            _isAlive = false;
            this.Die();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isReadyToFire) // detect mouse 1 down
        {
            _projectileChargeStartTime = Time.time; // stores value of the time when mouse 1 was down
        }
        else if (Input.GetMouseButtonUp(0) && _isReadyToFire)
        {
            _projectileChargeDuration = Mathf.Clamp((_projectileChargeDuration = Time.time - _projectileChargeStartTime), projectileBaseDamage, playerMaxProjectileCharge); // calculates charge duration and clamps within set value of parameters
            _projectileCalculatedDamage = projectileBaseDamage * _projectileChargeDuration; // calculates damage
            Shoot(_projectileCalculatedDamage); // shoots bullet
            _projectileChargeDuration = 0f; // resets charge 
            StartCoroutine(ShootingCooldown()); // start cooldown
        }
    }

    IEnumerator ShootingCooldown()
    {
        _isReadyToFire = false; // player temporarily unable to fire
        yield return new WaitForSeconds(shootingRate); // cooldown 
        _isReadyToFire = true; // player can now fire
    }
    
    void Shoot(float projectileDamage)
    { // spawn object and assign it to a gameobject as reference
        _spawnedObject = Instantiate(playerProjectile, transform.position, transform.rotation);
        _spawnedObjectScript = _spawnedObject.GetComponent<projectileScript>(); // get projectile script of spawned object
        _spawnedObjectScript.Init(projectileSpeed, projectileDamage, projectileDespawnRate); // pass through variables
        Debug.Log("Spawned Bullet with a damage value of <b>" + projectileDamage + "<\b>");
        Damage(projectileHealthConsumption); // damage player
    }

    void Damage(float damageAmount) // damage function
    {
        _playerHealth -= damageAmount; // reduces health by damage value passed through 
    }
    
    void Die()
    { // destroys player gameobject
        Destroy(this.gameObject);
    }
}
