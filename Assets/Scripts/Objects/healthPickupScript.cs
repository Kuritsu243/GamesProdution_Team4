using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickupScript : MonoBehaviour
{
    [SerializeField] private float healAmount;
    private GameObject _player;
    private GameObject _collidedObject;
    private playerHealth _playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<playerHealth>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided object");
        _collidedObject = other.gameObject;
        Debug.Log(_collidedObject);
        if (_collidedObject.CompareTag("Player") || _collidedObject.CompareTag("lightRadius") || _collidedObject.CompareTag("playerCapsule"))
        {
            _playerHealth.PlayerHealth += healAmount;
            Despawn();
        }
    }

    void Despawn()
    {
        Destroy(gameObject);
    }
}
