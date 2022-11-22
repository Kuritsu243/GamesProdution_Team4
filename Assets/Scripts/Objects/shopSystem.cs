using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] shopItems;
    [SerializeField] private int rangeToSpawn;
    [SerializeField] private int minItemCost;
    [SerializeField] private int maxItemCost;
    private GameObject _player;
    private shopPawn[] _shopPawns;
    private playerHealth _playerHealth;
    private int _countOfShopItems;
    private int _itemCost;
    private float _distanceToPlayer;
    private bool _hasSpawnedItems;

    public GameObject[] ShopItems => shopItems;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<playerHealth>();
        _shopPawns = GetComponentsInChildren<shopPawn>();
        _countOfShopItems = shopItems.Length;
    }

    private void Update()
    {
        if (IsNearPlayer() && !_hasSpawnedItems) // if player is near and items haven't spawned
        {
            SpawnItems(); // spawn items
            _hasSpawnedItems = true; // items have now been spawned
        }
    }

    private bool IsNearPlayer() // returns true or false whether if player is in range
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        return _distanceToPlayer < rangeToSpawn;
    }

    private void SpawnItems() // spawn items
    {
        foreach(shopPawn pawn in _shopPawns) // for every shop pawn childed to the shop system
        {
            pawn.GenerateItem(Random.Range(0, _countOfShopItems)); // randomize number between 0 and the size of array
            pawn.CalculateCost(Random.Range(minItemCost, maxItemCost)); // randomize number between mix and max cost
        }
    }

    public void PlayerHasCollectedItem(int damage) // if player has selected item
    {
        foreach (shopPawn pawn in _shopPawns)
        {
            if (pawn.isSelectedByPlayer) // checks if selected by player
            {
                Destroy(pawn.gameObject); // destroy self
                _playerHealth.Damage(pawn.cost); // damage player
            }
            else
            {
                pawn.DestroySelfAndItem(); // destroy pawn if not selected
            }
        
        }
    }

}
