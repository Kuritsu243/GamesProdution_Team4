using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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


public class playerShooting : MonoBehaviour
{
    [SerializeField] private float projectileBaseDamage;
    [SerializeField] private float projectileMaxCharge;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDespawnRate;
    [SerializeField] private float projectileCooldown;
    [SerializeField] private float projectileCost;
    [SerializeField] private GameObject playerProjectile;
    private inputSystem _inputSystem;
    private float _projectileCharge;
    public float projectileChargeDuration;
    private float _projectileChargeStartTime;
    private float _projectileCalculatedDamage;
    private bool _canFire = true;
    private GameObject _spawnedObject;
    private GameObject _projectileSpawnPoint;
    private projectileScript _spawnedObjectScript;
    private playerHealth _playerHealth;
    private DontDestroy _dontDestroy;


    public bool IsCharging { get; private set; }

    public float ProjectileMaxCharge => projectileMaxCharge;

    // Start is called before the first frame update
    private void Awake()
    {
        _inputSystem = GetComponent<inputSystem>();
        _playerHealth = GetComponent<playerHealth>();
        _projectileSpawnPoint = gameObject.FindGameObjectInChildWithTag("projectileSpawnPoint");
        _dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();

        projectileMaxCharge += _dontDestroy.ChargeSizeUpgradesPurchased;
        projectileBaseDamage += _dontDestroy.DamageUpgradesPurchased;
        

    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_inputSystem.mouseFire && _canFire && !IsCharging)
        {
            _projectileChargeStartTime = Time.time;
            IsCharging = true;
        }
        else if (!_inputSystem.mouseFire && _canFire && IsCharging)
        {
            _projectileCalculatedDamage = projectileBaseDamage * projectileChargeDuration;
            if (_projectileCalculatedDamage < 1)
            {
                _projectileCalculatedDamage = 1;
            }
            Shoot(_projectileCalculatedDamage);
            StartCoroutine(FiringCooldown());
            IsCharging = false;
        }
        else if (_projectileChargeStartTime >= 0 && IsCharging)
        {
            projectileChargeDuration = Time.time - _projectileChargeStartTime;
        }

        
    }

    private IEnumerator FiringCooldown()
    {
        _canFire = false;
        yield return new WaitForSeconds(projectileCooldown);
        _canFire = true;
    }

    public void Shoot(float projectileDamage)
    { // spawn object and assign it to a gameobject as reference
        _projectileCharge = Mathf.Clamp(projectileChargeDuration + projectileBaseDamage, projectileBaseDamage, projectileMaxCharge); // min value is base damage, max value is max charge value
        _spawnedObject = Instantiate(playerProjectile, _projectileSpawnPoint.transform.position, transform.rotation);
        _spawnedObjectScript = _spawnedObject.GetComponentInChildren<projectileScript>(); // get projectile script of spawned object
        _spawnedObjectScript.Init(projectileSpeed, projectileDamage, projectileDespawnRate, _projectileCharge, transform.localRotation); // pass through variables
        _playerHealth.Damage(projectileDamage);
    }
}