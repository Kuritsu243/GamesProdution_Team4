using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class playerSpriteRotation : MonoBehaviour
{
    private GameObject _player;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;
    private playerAnimation _playerAnimation;
    private const int HeightOffset = 2;
    private string _currentFrame;
    private int _frameHeightOffset;
    private Vector3 _playerPos;
    private Vector3 _newPos;
    private Vector3 _airbornePos;
    private float _airborneOffset;
    private float _spriteHeight;
    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimation = GetComponent<playerAnimation>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _currentFrame = _playerAnimation.GetCurrentFrame();
        _playerPos = _playerTransform.position;
        if (IsAirborne(_currentFrame))
        {
            // get airborne height depending on frame
            _airborneOffset = _playerPos.y + UpdatePosBasedOnFrame(_currentFrame);
            // set airborne position
            _airbornePos.x = 0f;
            _airbornePos.y = _airborneOffset;
            _airbornePos.z = 0f;
            // set sprite height to lerped vector with added offset
            _spriteHeight = Vector3.Lerp(_playerPos, _airbornePos, Time.deltaTime).y + HeightOffset;
            // set new pos vector to player position + calculated sprite height
            _newPos.x = _playerPos.x;
            _newPos.y = _spriteHeight;
            _newPos.z = _playerPos.z;
            // apply new position
            transform.position = _newPos;
        }
        else if (!IsAirborne(_currentFrame))
        {
            transform.position = new Vector3(_playerPos.x, _playerPos.y + HeightOffset, _playerPos.z);
        }
        
        _spriteRenderer.flipX = (_playerTransform.eulerAngles.y > 180f);
    }

    private int UpdatePosBasedOnFrame(string currentFrame)
    {
        _frameHeightOffset = currentFrame switch
        {
            "8" => 1,
            "10" => 2,
            "12" => 3,
            "14" => 2,
            _ => 0
        };

        return _frameHeightOffset + HeightOffset;
    }

    private static bool IsAirborne(string currentFrame)
    {
        return currentFrame switch
        {
            "8" => true,
            "10" => true,
            "12" => true,
            "14" => true,
            _ => false
        };
    }
}
