using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mothTrigger : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private GameObject[] mothSpawnersToBeTriggered;

    // Start is called before the first frame update
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player

        foreach (GameObject mothSpawner in mothSpawnersToBeTriggered) // for each moth spawner
        {
            mothSpawner.GetComponent<mothSpawnController>().SpawnEnemies = false; // disable spawning
        }
    }
    private void OnTriggerEnter(Collider other) // on collision
    {
#pragma warning disable CS0642
        if (!other.gameObject == _player); // if object isn't player then return
#pragma warning restore CS0642
        foreach (var spawnController in mothSpawnersToBeTriggered) // for each moth spawner
        {
            spawnController.GetComponent<mothSpawnController>().SpawnEnemies = true; // enable spawning
        }
    }

}
