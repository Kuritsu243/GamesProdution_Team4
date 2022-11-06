using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObject : MonoBehaviour
{
    // vars accessible in editor
    [SerializeField] private float timeToTake;
    [SerializeField] private Vector3 destinationPos;
    // private vars
    private float _distance;
    private Vector3 _direction;
    // public vars
    public bool hasConditionBeenMet;

    private void FixedUpdate()
    {
        if (!hasConditionBeenMet) return;
        _direction = destinationPos - transform.position;
        _distance = _direction.magnitude;
        transform.Translate(_direction.normalized * (Time.deltaTime * _distance / timeToTake), Space.World); // move object to target in world space
    }
}
