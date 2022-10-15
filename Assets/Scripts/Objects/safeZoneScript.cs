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
    [SerializeField] private float safeZoneRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
        _renderWithinRadius = _player.GetComponentInChildren<renderWithinRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_playerTransform.position, this.transform.position) < safeZoneRadius)
        {
            _renderWithinRadius.IsWithinSafeZone = true;
        }
        else
        {
            _renderWithinRadius.IsWithinSafeZone = false;
        }
    }
}
