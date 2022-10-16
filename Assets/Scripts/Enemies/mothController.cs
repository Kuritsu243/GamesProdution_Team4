using System;
using System.Collections;
using System.Collections.Generic;
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
    

    // private variables
    private GameObject _player;

    void Start()
    {
        Physics.IgnoreLayerCollision(8, 9, true);
        _eventManager = GameObject.FindGameObjectWithTag("eventManager"); // find event manager
        _lightDecoyController = _eventManager.GetComponent<lightDecoyController>(); // find light controller script
        _player = GameObject.FindGameObjectWithTag("Player"); // find player
        _playerHealth = _player.GetComponent<playerHealth>();
        _mothNavMeshAgent = GetComponent<NavMeshAgent>(); // get AI nav component
    }

    void Update()
    {
        if (_isTargetingPlayer)
        {
            _mothNavMeshAgent.destination = _player.transform.position; // set target to player location
        }

        if (enemyHealth <= 0)
        {
            Die();
        }

        StartCoroutine(CheckForDecoys()); // check for decoys periodically
    }

    void CheckSurroundings()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereCastRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("activeDecoy"))
            {
                _detectedActiveDecoy = hitCollider.gameObject;
                _detectedDecoyScript = _detectedActiveDecoy.GetComponent<lightDecoyPawn>();
                TargetDecoy();
            }
            else if (hitCollider.CompareTag("recentlyDisabledDecoy"))
            {
                TargetPlayer();
            }
        }
    }

    IEnumerator CheckForDecoys() // check for decoys
    {
        yield return new WaitForSeconds(checkSurroundingFrequency); // wait for the set period of time
        CheckSurroundings();
    }

    void TargetDecoy() // target lit decoy
    {
        _mothNavMeshAgent.destination = _detectedActiveDecoy.transform.position; // set target to lit decoy pos
        _isTargetingPlayer = false; // no longer targeting player
    }

    void TargetPlayer()
    {
        _isTargetingPlayer = true; // targets player
    }

    void Die()
    {
        Destroy(this.gameObject); // despawn
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collidedObject = collision.gameObject;
        if (_collidedObject.CompareTag("Player") && _canDealDamage)
        {
            _playerHealth.PlayerHealth -= damageAmount;
        }
    }

    private IEnumerator DamageCooldown(float cooldown)
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