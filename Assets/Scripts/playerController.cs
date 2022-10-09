using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Everytime you're outside a zone the light radius shrinks until you reach a safe zone - if light radius reaches zero then player dies
// TODO: the lower the health the lower the light of the player
// TODO: scale fuel with projectile charge
// TODO: Regain life from found candles / defeated moths

public static class Helper
{
    public static GameObject FindGameObjectInChildWithTag(this GameObject gameObject, string tag) // creates function to allow to search for a gameobject with a tag
    {
        Transform t = gameObject.transform;
        foreach (Transform transform in t)
        {
            if (transform.CompareTag(tag))
            {
                return transform.gameObject;
            }
        }
        return null;
    }
}




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
    private bool _isAlive = true;
    private bool _isCharging;
    private bool _isReadyToFire = true;

    //Private variables
    private float _playerHealth;
    private float _projectileCalculatedDamage;
    private float _projectileCharge;
    private float _projectileChargeDuration = 0f;
    private float _projectileChargeStartTime;
    private GameObject _projectileSpawnPoint;
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
        _projectileSpawnPoint = gameObject.FindGameObjectInChildWithTag("projectileSpawnPoint"); // find spawn point by tag in child
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isReadyToFire && !_isCharging) // detect mouse 1 down
        {
            _projectileChargeStartTime = Time.time; // stores value of the time when mouse 1 was down
            _isCharging = true;
            // calculates charge duration and clamps within set value of parameters
        }
        else if (Input.GetMouseButtonUp(0) && _isReadyToFire)
        {
        
            _projectileCalculatedDamage = projectileBaseDamage * _projectileChargeDuration; // calculates damage
            Shoot(_projectileCalculatedDamage); // shoots bullet
            StartCoroutine(ShootingCooldown());// start cooldown
            _isCharging = false;  
        }
        else if (_projectileChargeStartTime >= 0 && _isCharging)
        {
            _projectileChargeDuration = Time.time - _projectileChargeStartTime;
        }
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

    IEnumerator ShootingCooldown()
    {
        _isReadyToFire = false; // player temporarily unable to fire
        yield return new WaitForSeconds(shootingRate); // cooldown 
        _isReadyToFire = true; // player can now fire
        _projectileChargeStartTime = 0f;
        _projectileChargeDuration = 0f;

    }

    void Shoot(float projectileDamage)
    { // spawn object and assign it to a gameobject as reference
        _projectileCharge = Mathf.Clamp(_projectileChargeDuration + projectileBaseDamage, projectileBaseDamage, playerMaxProjectileCharge); // min value is base damage, max value is max charge value
        _spawnedObject = Instantiate(playerProjectile, _projectileSpawnPoint.transform.position, transform.rotation);
        _spawnedObjectScript = _spawnedObject.GetComponent<projectileScript>(); // get projectile script of spawned object
        _spawnedObjectScript.Init(projectileSpeed, projectileDamage, projectileDespawnRate, _projectileCharge); // pass through variables
        Debug.Log("Spawned Bullet with a damage value of " + projectileDamage);
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

