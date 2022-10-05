using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private float burningRate;
    [SerializeField] private float maxPlayerHealth = 100;
    private float _playerHealth;
    private bool _isAlive = true;
    
    // Encapsulated Fields
    public float PlayerHealth
    {
        get => _playerHealth;
        private set => _playerHealth = value;
    }
    public float MaxPlayerHealth
    {
        get => maxPlayerHealth;
        set => maxPlayerHealth = value;
    }

    
    void Start()
    {
        PlayerHealth = MaxPlayerHealth;
    }
    private void FixedUpdate()
    {
        if (_isAlive)
        { 
            _playerHealth = Mathf.Clamp(_playerHealth - (Time.deltaTime * burningRate), 0f, MaxPlayerHealth);
        }
        if (_playerHealth  <= 0)
        {
            _isAlive = false;
            this.Die();
        }
    }


    void Die()
    {
        Destroy(this.gameObject);
    }
}
