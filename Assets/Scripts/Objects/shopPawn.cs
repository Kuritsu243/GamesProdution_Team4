using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public class shopPawn : MonoBehaviour
{
    private shopSystem _shopSystem;
    private GameObject[] _shopItems;
    private GameObject _selectedItem;
    private GameObject _spawnedItem;
    private Transform _pawnTransform;
    private TextMeshProUGUI _costText;
    public int cost;
    public bool isSelectedByPlayer;
    // Start is called before the first frame update
    private void Start()
    {
        _shopSystem = GetComponentInParent<shopSystem>();
        _shopItems = _shopSystem.ShopItems;
        _pawnTransform = transform;
        _costText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void GenerateItem(int range) // randomise item to create
    {
        _selectedItem = _shopItems[range]; // get number of object to spawn
        var position = _pawnTransform.position; // set pos
        _spawnedItem = Instantiate(_selectedItem, new Vector3(position.x, position.y + 1f, position.z),
            quaternion.identity, transform); // instantiate shop item
        
    }

    private void OnTriggerEnter(Collider other) // on collision
    {
        Debug.Log(other.gameObject);
        if (!other.CompareTag("Player") && !other.CompareTag("lightRadius") &&
            !other.CompareTag("playerCapsule")) return; // if not player
        isSelectedByPlayer = true; // if player chooses pawn's object
        _shopSystem.PlayerHasCollectedItem(cost); // remove the cost from player health
    }

    public void CalculateCost(int damage) // calculate damage
    {
        cost = damage;
        _costText.text = cost.ToString(); // show cost in UI
    }

    public void DestroySelfAndItem() // destroys self and spawned object 
    {
        Destroy(_spawnedItem);
        Destroy(gameObject);
    }
}
