using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCrosshair : MonoBehaviour
{
    [SerializeField] private float crosshairDistanceFromPlayer;

    private GameObject _player;
    private Transform _playerTransform;

    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        var playerPos = _player.transform.position;
        var crosshairPos = transform.position;
        transform.position = new Vector3(playerPos.x + crosshairDistanceFromPlayer, transform.position.y,
            playerPos.z);
    }
}
