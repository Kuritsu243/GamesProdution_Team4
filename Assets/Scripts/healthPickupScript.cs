using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickupScript : MonoBehaviour
{
    [SerializeField] private float healAmount;
    private GameObject _player;
    private GameObject _collidedObject;
    private playerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<playerController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided object");
        _collidedObject = other.gameObject;
        if (_collidedObject.CompareTag("Player") || _collidedObject.CompareTag("lightRadius"))
        {
            _playerController.PlayerHealth += healAmount;
            Despawn();
        }
    }

    void Despawn()
    {
        Destroy(gameObject);
    }
}
