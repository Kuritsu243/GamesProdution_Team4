using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeZoneScript : MonoBehaviour
{

    private GameObject _player;
    private playerController _playerController;
    private Transform _playerTransform;
    private Vector3 _playerPos;
    private renderWithinRadius _renderWithinRadius;
    private float _safeZoneRadius;

    public float SafeZoneRadius
    {
        set => _safeZoneRadius = value;
        get => _safeZoneRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<playerController>();
        _playerTransform = _player.transform;
        _renderWithinRadius = _player.GetComponentInChildren<renderWithinRadius>();
        _safeZoneRadius = _playerController.SafeZoneRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_playerTransform.position, this.transform.position) < _playerController.SafeZoneRadius)
        {
            _renderWithinRadius.IsWithinSafeZone = true;
        }
        else
        {
            _renderWithinRadius.IsWithinSafeZone = false;
        }
    }
}
