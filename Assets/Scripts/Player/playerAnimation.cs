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
    private bool _isMoving;
    private const string RegexPattern = @"\d+";
    private Transform _spriteTransform;
    private Vector3 _spritePos;
    [SerializeField] private AnimationClip movingAnim;
    [SerializeField] private AnimationClip idleAnim;

    public Animator PlayerAnimator { get; private set; }

    private void Start()
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
        _isPlayerMoving = _playerMovement.IsMoving; // if player is moving
        switch (_isPlayerMoving) // check the conditions
        {
            case false:
                PlayerAnimator.Play("playerIdle"); // play idle anim
                PlayerAnimator.speed = 1f;
                break;
            case true:
                PlayerAnimator.Play("playerAnimation"); // play moving anim
                PlayerAnimator.speed = 1f;
                break;
        }
        


    }
    public string GetCurrentFrame() // gets current frame of anim
    {
        _currentFrame = _playerSpriteRenderer.sprite.name; // current frame = the name of the frame
        _currentFrame = Regex.Match(_currentFrame, RegexPattern).Value; // removes text via regex, returns only a number
        return _currentFrame; // return frame number
    }

}
