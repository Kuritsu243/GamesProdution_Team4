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
    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        var position = _playerTransform.position;
        transform.position = new Vector3(position.x, position.y + 2f, position.z);
        _spriteRenderer.flipX = !(_playerTransform.eulerAngles.y > 180f);
    }
}
