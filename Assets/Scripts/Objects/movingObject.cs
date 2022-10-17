using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObject : MonoBehaviour
{
    [SerializeField] private float timeToTake;
    [SerializeField] private Vector3 destinationPos;
    private float _distance;
    private Vector3 _direction;
    public bool hasConditionBeenMet;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasConditionBeenMet)
        {
            _direction = destinationPos - transform.position;
            _distance = _direction.magnitude;
            transform.Translate(_direction.normalized * (Time.deltaTime * _distance / timeToTake));
        }
    }
}
