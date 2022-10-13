using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController2 : MonoBehaviour
{
    private inputSystem _inputSystem;
    private playerMovement _playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        _inputSystem = GetComponent<inputSystem>();
        _playerMovement = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _inputSystem.HandleAllInputs();
        _playerMovement.HandleAllMovement();
    }
}
