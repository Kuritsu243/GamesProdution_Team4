using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;


public class mothController : MonoBehaviour
{
    // serialized fields
    [SerializeField] private float enemyHealth;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float damageAmount;
    [SerializeField] private float checkSurroundingFrequency;
    [SerializeField] private float timeInBetweenDealingDamage;
    [SerializeField] private GameObject healthObject;
    private GameObject _detectedActiveDecoy;
    private GameObject _collidedObject;
    private lightDecoyPawn _detectedDecoyScript;
    private GameObject _eventManager;
    private gameSaturationModifier _gameSaturationModifier;
    private bool _isTargetingPlayer = true;
    private bool _canDealDamage = true;
    private lightDecoyController _lightDecoyController;
    private NavMeshAgent _mothNavMeshAgent;
    private playerHealth _playerHealth;
    private Camera _playerCamera;
    

    // private variables
    private GameObject _player;

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 9, true); // ignore collisions with other enemies and decoys
        _eventManager = GameObject.FindGameObjectWithTag("eventManager"); // find event manager
        _lightDecoyController = _eventManager.GetComponent<lightDecoyController>(); // find light controller script
        _player = GameObject.FindGameObjectWithTag("Player"); // find player
        _playerHealth = _player.GetComponent<playerHealth>(); // get player health script from player
        _mothNavMeshAgent = GetComponent<NavMeshAgent>(); // get AI nav component
        _playerCamera = _player.transform.parent.GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        if (_isTargetingPlayer) // if currently targeting player
        {
            _mothNavMeshAgent.destination = _player.transform.position; // set target to player location
        }

        if (enemyHealth <= 0.01f) // if enemy health is 0
        {
            Die(); // despawn enemy
        }

        StartCoroutine(CheckForDecoys()); // check for decoys periodically
    }

    private void LateUpdate()
    {
        transform.LookAt(_playerCamera.transform);
    }

    private void CheckSurroundings()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereCastRadius); // store all hit colliders in a var
        foreach (var hitCollider in hitColliders) // for each collider that was hit
        {
            if (hitCollider.CompareTag("activeDecoy")) // if tag is an active decoy
            {
                _detectedActiveDecoy = hitCollider.gameObject;
                _detectedDecoyScript = _detectedActiveDecoy.GetComponent<lightDecoyPawn>();
                TargetDecoy(); // target the decoy object instead of player
            }
            else if (hitCollider.CompareTag("recentlyDisabledDecoy"))
            {
                TargetPlayer(); // if decoy is disabled then target player
            }
        }
    }

    private IEnumerator CheckForDecoys() // check for decoys
    {
        yield return new WaitForSeconds(checkSurroundingFrequency); // wait for the set period of time
        CheckSurroundings();
    }

    private void TargetDecoy() // target lit decoy
    {
        _mothNavMeshAgent.destination = _detectedActiveDecoy.transform.position; // set target to lit decoy pos
        _isTargetingPlayer = false; // no longer targeting player
    }

    private void TargetPlayer()
    {
        _isTargetingPlayer = true; // targets player
    }

    private void Die()
    {
        var healthPickup = Instantiate(healthObject, transform.position, Quaternion.identity);
        Debug.Log("moth death");
        Destroy(this.gameObject); // despawn
    }
    private void OnCollisionEnter(Collision collision)
    {
        _collidedObject = collision.gameObject;
        if (_collidedObject.CompareTag("Player") && _canDealDamage)
        {
            _playerHealth.PlayerHealth -= damageAmount; // deal damage to player
        }
    }

    private IEnumerator DamageCooldown(float cooldown) // cooldown for damaging the enemy
    {
        _canDealDamage = false;
        yield return new WaitForSeconds(cooldown);
        _canDealDamage = true;
    }
    

    public void Damage(float amount) // take damage 
    {
        enemyHealth -= amount;
        StartCoroutine(DamageCooldown(timeInBetweenDealingDamage));
    }
}