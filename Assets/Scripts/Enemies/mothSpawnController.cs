using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class mothSpawnController : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] private GameObject[] mothPrefabs;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float spawnAmount;
    private GameObject _selectedPrefab;
    private int _numberOfMothPrefabs;
    private int _numberOfSpawnPoints;
    private bool _onCooldown;
    public bool spawnEnemies;
    private void Awake()
    {
        _numberOfMothPrefabs = mothPrefabs.Length;
        _numberOfSpawnPoints = spawnPoints.Length;
    }

    private void Update()
    {
        if (spawnEnemies && !_onCooldown)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (!spawnPoint.GetComponent<mothSpawnPawn>().isEnabled) continue;
            for (var i = 0; i < spawnAmount; i++)
            {
                var position = spawnPoint.transform.position;
                var spawnedObject = Instantiate(mothPrefabs[RandomizeMothPrefab()], new Vector3(position.x, position.y + 2f, position.z),
                    quaternion.identity);
            }
        }

        StartCoroutine(SpawnCooldown());
    }

    public void DisableSpawnPoint()
    {

    }

    private int RandomizeMothPrefab()
    {
        return Random.Range(0, _numberOfMothPrefabs);
    }

    private IEnumerator SpawnCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(spawnCooldown);
        _onCooldown = false;
    }
    

}
