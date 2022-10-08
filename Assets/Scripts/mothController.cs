using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class mothController : MonoBehaviour
{
    // serialized fields
    [SerializeField] private float enemyHealth;
    [SerializeField] private float sphereCastRadius;

    [SerializeField] private float checkSurroundingFrequency;
    private GameObject _detectedActiveDecoy;
    private lightDecoyPawn _detectedDecoyScript;
    private GameObject _eventManager;
    private gameSaturationModifier _gameSaturationModifier;
    private bool _isTargetingPlayer = true;
    private lightDecoyController _lightDecoyController;
    private NavMeshAgent _mothNavMeshAgent;

    // private variables
    private GameObject _player;

    void Start()
    {
        _eventManager = GameObject.FindGameObjectWithTag("eventManager"); // find event manager
        _lightDecoyController = _eventManager.GetComponent<lightDecoyController>(); // find light controller script
        _player = GameObject.FindGameObjectWithTag("Player"); // find player
        _mothNavMeshAgent = GetComponent<NavMeshAgent>(); // get AI nav component
        _gameSaturationModifier =
            _eventManager.GetComponent<gameSaturationModifier>(); // find saturation modifier script
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
        _gameSaturationModifier.EnemiesCurrentlyInScene--; // decrement enemies value
        _gameSaturationModifier.CalculateSaturationLevel(); // update saturation level
        Destroy(this.gameObject); // despawn
    }

    public void Damage(float amount) // take damage 
    {
        enemyHealth -= amount;
    }
}