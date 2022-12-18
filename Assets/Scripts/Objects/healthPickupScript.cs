using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickupScript : MonoBehaviour
{
    // vars accessible in editor
    [SerializeField] private float healAmount;
    // private vars
    private GameObject _player;
    private GameObject _collidedObject;
    private playerHealth _playerHealth;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player
        _playerHealth = _player.GetComponent<playerHealth>(); // get player health
    }
    
    private void OnTriggerEnter(Collider other) // on collision
    {
        _collidedObject = other.gameObject;
        if (!_collidedObject.CompareTag("Player") && !_collidedObject.CompareTag("lightRadius") &&
            !_collidedObject.CompareTag("playerCapsule")) return;
        _playerHealth.PlayerHealth += healAmount; // heal player
        Despawn(); // destroy object
    }

    void Despawn()
    {
        Destroy(gameObject);
    }
}
