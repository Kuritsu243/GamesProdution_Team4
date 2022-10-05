using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasScript : MonoBehaviour
{
    private GameObject _player;
    private playerController _playerController;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBackground;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    { 
        healthBar.fillAmount = _playerController.PlayerHealth / _playerController.MaxPlayerHealth;
    }
}
