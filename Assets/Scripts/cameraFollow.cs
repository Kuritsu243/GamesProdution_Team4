using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    private Vector3 _cameraStartingPosition;

    private void Start()
    {
        _cameraStartingPosition = transform.position;
    }

    public void MoveCamera(float cameraHeight, float zOffset, Vector3 playerPos)
    {
        transform.position = new Vector3(playerPos.x, cameraHeight, playerPos.z - zOffset);
    }
}
