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
    public void GenerateItem(int range)
    {
        _selectedItem = _shopItems[range];
        var position = _pawnTransform.position;
        _spawnedItem = Instantiate(_selectedItem, new Vector3(position.x, position.y + 1f, position.z),
            quaternion.identity, transform);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (!other.CompareTag("Player") && !other.CompareTag("lightRadius") &&
            !other.CompareTag("playerCapsule")) return;
        isSelectedByPlayer = true;
        _shopSystem.PlayerHasCollectedItem(cost);
    }

    public void CalculateCost(int damage)
    {
        cost = damage;
        _costText.text = cost.ToString();
    }

    public void DestroySelfAndItem()
    {
        Destroy(_spawnedItem);
        Destroy(gameObject);
    }
}
