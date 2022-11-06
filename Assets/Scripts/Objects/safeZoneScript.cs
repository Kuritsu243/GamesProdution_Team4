using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeZoneScript : MonoBehaviour
{
    // vars accessible in editor
    [SerializeField] private float safeZoneRadius;
    // private vars
    private GameObject _player;
    private playerController _playerController;
    private Transform _playerTransform;
    private Vector3 _playerPos;
    private renderWithinRadius _renderWithinRadius;

    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player
        _playerTransform = _player.transform; // get player transform
        _renderWithinRadius = _player.GetComponentInChildren<renderWithinRadius>(); // get render script
    }
    
    private void Update()
    {
        _renderWithinRadius.IsWithinSafeZone = Vector3.Distance(_playerTransform.position, this.transform.position) < safeZoneRadius; //
    }
}
