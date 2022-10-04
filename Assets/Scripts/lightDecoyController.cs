using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDecoyController : MonoBehaviour
{
    
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float decoyLitDuration;
    private GameObject _player;
    private GameObject _decoyInRange;
    private CharacterController _characterController;
    private lightDecoyPawn _decoyPawnScript;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = _player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, sphereCastRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("decoy"))
                {
                    _decoyInRange = hitCollider.gameObject;
                    _decoyPawnScript = _decoyInRange.GetComponent<lightDecoyPawn>();
                    _decoyPawnScript.ActivateDecoy(decoyLitDuration);
                }

            }
        }
    }


}
