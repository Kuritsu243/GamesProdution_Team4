using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasScript : MonoBehaviour
{
    private GameObject _player;
    private playerController _playerController;

    // serialized fields that can be accessed in the inspector
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBackground;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player gameobject
        _playerController = _player.GetComponent<playerController>(); // get player controller component from player gameobject
    }
    void Update()
    { 
        healthBar.fillAmount = _playerController.PlayerHealth / _playerController.MaxPlayerHealth; // fills the health bar image by how much health the player has compared to their max health
    }
}
