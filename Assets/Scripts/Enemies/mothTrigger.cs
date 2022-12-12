using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class mothTrigger : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private GameObject[] mothSpawnersToBeTriggered;
    [SerializeField] private GameObject[] mothSpawnersToBeDisabled;
    [SerializeField] private GameObject[] movingObjectsToTrigger;
    [SerializeField] private bool randomizeSpawning;
    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player

        foreach (GameObject mothSpawner in mothSpawnersToBeTriggered) // for each moth spawner
        {
            mothSpawner.GetComponent<mothSpawnController>().SpawnEnemies = false; // disable spawning
        }
        
    }
    private void OnTriggerEnter(Collider other) // on collision
    {
        Debug.Log(other);
#pragma warning disable CS0642
        if (!other.gameObject == _player) return; // if object isn't player then return
#pragma warning restore CS0642
        if (randomizeSpawning)
        {
            foreach (var spawnController in mothSpawnersToBeTriggered)
            {
                spawnController.GetComponent<mothSpawnController>().RandomizeSpawning = true;
            }
        }
        foreach (var movingObject in movingObjectsToTrigger)
        {
            Debug.Log(movingObject.name);
            movingObject.GetComponent<movingObject>().haveConditionsBeenMet = true;
        }
        
        foreach (var spawnController in mothSpawnersToBeTriggered) // for each moth spawner
        {
            
            spawnController.GetComponent<mothSpawnController>().SpawnEnemies = true; // enable spawning
        }

        if (mothSpawnersToBeDisabled == null || mothSpawnersToBeDisabled.Length == 0) return;
        
        foreach (var spawnController in mothSpawnersToBeDisabled)
        {
            spawnController.GetComponent<mothSpawnController>().SpawnEnemies = false;
        }
        //
        // if (movingObjectsToTrigger == null || movingObjectsToTrigger.Length == 0) return;

        
    }

}
