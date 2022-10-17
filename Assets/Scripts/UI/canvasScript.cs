using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasScript : MonoBehaviour
{
    private GameObject _player;
    private playerHealth _playerHealth;
    private playerShooting _playerShooting;

    // serialized fields that can be accessed in the inspector
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBackground;
    [SerializeField] private Image chargeBar;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player gameobject
        _playerHealth = _player.GetComponent<playerHealth>();
        _playerShooting =
            _player.GetComponent<playerShooting>(); // get player controller component from player gameobject
    }
    void FixedUpdate()
    {
        healthBar.fillAmount = _playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth;
        // fills the health bar image by how much health the player has compared to their max health
        if (_playerShooting.IsCharging)
        {
            chargeBar.fillAmount = _playerShooting.projectileChargeDuration / _playerShooting.ProjectileMaxCharge;
        }
        else if (!_playerShooting.IsCharging)
        {
            chargeBar.fillAmount = 0;
        }
    }
}
