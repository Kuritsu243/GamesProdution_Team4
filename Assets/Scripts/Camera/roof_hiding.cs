using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roof_hiding : MonoBehaviour
{
    // serialized variables
    [SerializeField] private float distanceBetweenRoof;

    // private variables
    private Camera _camera;
    private GameObject _hitObject;
    private bool _isRoofActive = true;
    private GameObject _player;
    private GameObject _previouslyDisabledObject;
    private GameObject _roof;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // find player
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // get main camera component
        
    }

    void Update()
    {
        CheckIfTouchingRoof(); // check if touching roof
        UpdateDistanceBetweenPlayerAndRoof(); // get distance between player and roof
        if (distanceBetweenRoof > 15f) // if out of range of roof
        {
            ToggleRoof(_previouslyDisabledObject); // enable roof
        }
    }

    void ToggleRoof(GameObject roof)
    {
        _isRoofActive = !_isRoofActive; 
        roof.SetActive(_isRoofActive); // enable / disable roof
        _previouslyDisabledObject = _hitObject; // store recently disabled roof as previously disabled
    }

    void CheckIfTouchingRoof()
    {
        if (Physics.Raycast(_camera.transform.position, (_player.transform.position - _camera.transform.position), out var hit)) // raycast, checks if roof is blocking a line between camera to player
        {
            _hitObject = hit.collider.gameObject; // object in between camera and player stored as var
            if (hit.collider.gameObject.CompareTag("roof")) // if object is roof
            {
                Debug.Log("<b> Hit Object: </b> " + _hitObject);
                _roof = _hitObject; // store collided object as roof
                ToggleRoof(_roof); // disable roof
            }
            else if (hit.collider.gameObject.tag == "") // if object has no tag
                Debug.Log("no tag, ignoring error");
        }
    }

    void UpdateDistanceBetweenPlayerAndRoof() 
    {
        distanceBetweenRoof = Vector3.Distance(_player.transform.position, _previouslyDisabledObject.transform.position); // calculates distance between player and roof
    }
}
