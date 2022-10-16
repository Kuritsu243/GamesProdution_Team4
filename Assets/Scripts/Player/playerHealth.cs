using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float playerMaxHealth;
    private float _playerHealth;
    private gameSaturationModifier _gameSaturationModifier;

    public float PlayerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }
    
    public float PlayerMaxHealth => playerMaxHealth;

    private void Awake()
    {
        _playerHealth = playerMaxHealth;
        _gameSaturationModifier =
            GameObject.FindGameObjectWithTag("eventManager").GetComponent<gameSaturationModifier>();
    }

    public void Damage(float damageAmount) // damage function
    {
        _playerHealth -= damageAmount; // reduces health by damage value passed through 
        _gameSaturationModifier.CalculateSaturationLevel();
    }

    private void FixedUpdate()
    {
        if (_playerHealth == 0)
        {
            Die();
        }
    }

    void Die()
    { // destroys player gameobject
        Destroy(this.gameObject);
    }
}
