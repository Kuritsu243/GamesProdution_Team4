using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDecoyController : MonoBehaviour
{
    // serialized variables
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float decoyLitDuration;
    private GameObject _decoyInRange;
    private lightDecoyPawn _decoyPawnScript;

    // private variables
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player gameobject
    }


}
