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
        if (IsNearPlayer() && !_hasSpawnedItems)
        {
            SpawnItems();
            _hasSpawnedItems = true;
        }
    }

    private bool IsNearPlayer()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        return _distanceToPlayer < rangeToSpawn;
    }

    private void SpawnItems()
    {
        foreach(shopPawn pawn in _shopPawns)
        {
            pawn.GenerateItem(Random.Range(0, _countOfShopItems));
            pawn.CalculateCost(Random.Range(minItemCost, maxItemCost));
        }
    }

    public void PlayerHasCollectedItem(int damage)
    {
        foreach (shopPawn pawn in _shopPawns)
        {
            if (pawn.isSelectedByPlayer)
            {
                Destroy(pawn.gameObject);
                _playerHealth.Damage(pawn.cost);
            }
            else
            {
                pawn.DestroySelfAndItem();
            }
        
        }
    }

}
