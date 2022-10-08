using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class mothController : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float checkSurroundingFrequency;
    private GameObject _player;
    private GameObject _detectedActiveDecoy;
    private GameObject _eventManager;
    private NavMeshAgent _mothNavMeshAgent;
    private lightDecoyPawn _detectedDecoyScript;
    private lightDecoyController _lightDecoyController;
    private gameSaturationModifier _gameSaturationModifier;
    private bool _isTargetingPlayer = true;
    
    void Start()
    {
        _lightDecoyController = GameObject.FindGameObjectWithTag("eventManager").GetComponent<lightDecoyController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _mothNavMeshAgent = GetComponent<NavMeshAgent>();
        _eventManager = GameObject.FindGameObjectWithTag("eventManager");
        _gameSaturationModifier = _eventManager.GetComponent<gameSaturationModifier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTargetingPlayer)
        {
            _mothNavMeshAgent.destination = _player.transform.position;
        }

        if (enemyHealth <= 0)
        {
            Die();
        }
        StartCoroutine(CheckForDecoys());
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

    IEnumerator CheckForDecoys()
    {
        yield return new WaitForSeconds(checkSurroundingFrequency);
        CheckSurroundings();
    }

    void TargetDecoy()
    {
        _mothNavMeshAgent.destination = _detectedActiveDecoy.transform.position;
        _isTargetingPlayer = false;
    }

    void TargetPlayer()
    {
        _isTargetingPlayer = true;
    }

    void Die()
    {
        _gameSaturationModifier.EnemiesCurrentlyInScene--;
        _gameSaturationModifier.CalculateSaturationLevel();
        Destroy(this.gameObject);
        
    }

    public void Damage(float amount)
    {
        enemyHealth -= amount;
    }

}
