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

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player gameobject
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F)) // if decoy activated
        // {
        //     Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, sphereCastRadius); // get array of colliders in range
        //     foreach (var hitCollider in hitColliders) // for each collider detected
        //     {
        //         if (hitCollider.CompareTag("decoy")) // if collider is decoy
        //         {
        //             _decoyInRange = hitCollider.gameObject; // store decoy as var
        //             _decoyPawnScript = _decoyInRange.GetComponent<lightDecoyPawn>(); // get decoy script
        //             _decoyPawnScript.ActivateDecoy(decoyLitDuration); // active decoy
        //         }
        //
        //     }
        // }
    }
}
