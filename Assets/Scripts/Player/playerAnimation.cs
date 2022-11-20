using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class playerAnimation : MonoBehaviour
{
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

    public Animator PlayerAnimator { get; private set; }

    private void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<playerMovement>();
        PlayerAnimator.speed = 1f;
        _spriteTransform = transform;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _isPlayerMoving = _playerMovement.IsMoving;
        switch (_isPlayerMoving)
        {
            case false:
                PlayerAnimator.Play("playerIdle");
                PlayerAnimator.speed = 1f;
                _clipWeight = PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                _clipLength = idleAnim.length;
                _clipFrameRate = idleAnim.frameRate;
                break;
            case true:
                PlayerAnimator.Play("playerAnimation");
                PlayerAnimator.speed = 1f;
                _clipWeight = PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                _clipLength = movingAnim.length;
                _clipFrameRate = movingAnim.frameRate;
                break;
        }
        


    }
    public string GetCurrentFrame()
    {
        _currentFrame = _playerSpriteRenderer.sprite.name;
        _currentFrame = Regex.Match(_currentFrame, RegexPattern).Value;
        return _currentFrame;
    }

}
