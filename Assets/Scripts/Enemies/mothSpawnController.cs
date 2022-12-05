using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class mothSpawnController : MonoBehaviour
{
    // vars accessible in editor
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] private GameObject[] mothPrefabs;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float spawnAmount;
    // private vars
    private GameObject _selectedPrefab;
    private int _numberOfMothPrefabs;
    private int _numberOfSpawnPoints;
    private bool _onCooldown;
    private bool _spawnEnemies;
    private bool _randomizeSpawning;

    public bool SpawnEnemies
    {
        get => _spawnEnemies;
        set => _spawnEnemies = value;
    }

    public bool RandomizeSpawning
    {
        get => _randomizeSpawning;
        set => _randomizeSpawning = value;
    }
    private void Awake()
    {
        _numberOfMothPrefabs = mothPrefabs.Length; // get number of prefabs
        _numberOfSpawnPoints = spawnPoints.Length; // get number of spawnpoints
    }

    private void Update()
    {
        if (_spawnEnemies && !_onCooldown) // if can spawn enemies
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_randomizeSpawning)
        {
            var chosenInt = Random.Range(0, _numberOfSpawnPoints);
            for (var i = 0; i < spawnAmount; i++)
            {
                var position = spawnPoints[chosenInt].transform.position;
                var spawnedObject = Instantiate(mothPrefabs[RandomizeMothPrefab()],
        new Vector3(position.x, position.y, position.z), quaternion.identity);
            }
        }
        else
        {
            foreach (var spawnPoint in spawnPoints) // for each spawn point
            {
                if (!spawnPoint.GetComponent<mothSpawnPawn>().isEnabled) continue; // if spawn point is enabled then
                for (var i = 0; i < spawnAmount; i++) // for how many moths the point should spawn
                {
                    var position = spawnPoint.transform.position; 
                    var spawnedObject = Instantiate(mothPrefabs[RandomizeMothPrefab()], new Vector3(position.x, position.y + 2f, position.z),
                        quaternion.identity);// spawn at spawn point pos
                }
            }
        }
        StartCoroutine(SpawnCooldown()); // start spawn cooldown
    }

    public void DisableSpawnPoint() // disable spawn point
    {

    }

    private int RandomizeMothPrefab() // randomize the moth variation to spawn
    {
        return Random.Range(0, _numberOfMothPrefabs);
    }

    private IEnumerator SpawnCooldown() // spawn cooldown
    {
        _onCooldown = true;
        yield return new WaitForSeconds(spawnCooldown);
        _onCooldown = false;
    }
    

}
