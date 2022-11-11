using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    private Animator _playerAnimator;
    private playerMovement _playerMovement;
    private GameObject _player;
    private bool _isPlayerMoving;
    
    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<playerMovement>();
        _playerAnimator.speed = 0.1f;
    }

    // Update is called once per frame
    private void Update()
    {
        _isPlayerMoving = _playerMovement.IsMoving;
        switch (_isPlayerMoving)
        {
            case false:
                _playerAnimator.Play("playerIdle");
                _playerAnimator.speed = 0.1f;
                break;
            case true:
                _playerAnimator.Play("playerAnimation");
                _playerAnimator.speed = 0.5f;
                break;
        }
    }
}
