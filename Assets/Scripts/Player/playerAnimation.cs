using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class playerAnimation : MonoBehaviour
{
    private Animator _playerAnimator;
    private playerMovement _playerMovement;
    private GameObject _player;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _isPlayerMoving;
    private string _currentFrame;
    private float _clipLength;
    private float _clipFrameRate;
    private float _clipWeight;
    private bool _isMoving;
    private const string RegexPattern = @"\d+";
    private Transform _spriteTransform;
    private Vector3 _spritePos;
    [SerializeField] private AnimationClip movingAnim;
    [SerializeField] private AnimationClip idleAnim;
    
    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<playerMovement>();
        _playerAnimator.speed = 1f;
        _spriteTransform = transform;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _isPlayerMoving = _playerMovement.IsMoving;
        switch (_isPlayerMoving)
        {
            case false:
                _playerAnimator.Play("playerIdle");
                _playerAnimator.speed = 1f;
                _clipWeight = _playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                _clipLength = idleAnim.length;
                _clipFrameRate = idleAnim.frameRate;
                break;
            case true:
                _playerAnimator.Play("playerAnimation");
                _playerAnimator.speed = 1f;
                _clipWeight = _playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                _clipLength = movingAnim.length;
                _clipFrameRate = movingAnim.frameRate;
                break;
        }
        
        _currentFrame = _playerSpriteRenderer.sprite.name;
        _currentFrame = Regex.Match(_currentFrame, RegexPattern).Value;
        
        _spritePos = _spriteTransform.position;
        switch (_currentFrame)
        {
            case "8":
                _spritePos.y += 1000f;
                break;
        }
        
        }
}
