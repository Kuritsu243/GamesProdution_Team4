using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScenePreview : MonoBehaviour
{
    [SerializeField] List<GameObject> followPoints = new List<GameObject>(); // creates a list of points
    [SerializeField] private bool isMoving; 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private GameObject _player;
    private GameObject _playerParent;
    private GameObject _canvas;
    private GameObject _cameraObject;
    private Camera _previewCamera;
    private Camera _playerCamera;
    private Transform _targetPosition;
    private Transform _currentPosition;

    private int _targetPoint;
    private int _numberOfPoints;

    public List<GameObject> FollowPoints // encapsulated field
    {
        get => followPoints;
        set => followPoints = value;
    }
    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player
        _playerParent = GameObject.FindGameObjectWithTag("playerParent"); // get player parent
        _canvas = GameObject.FindGameObjectWithTag("Canvas"); // get canvas
        _previewCamera = GetComponentInChildren<Camera>(); // get preview camera
        _cameraObject = _previewCamera.gameObject; // get preview camera game object
        _playerCamera = _player.GetComponentInChildren<Camera>(); // get player camera
        _numberOfPoints = followPoints.Count; // get number of points
        _playerParent.SetActive(false); // disable player GOs
        _canvas.SetActive(false); // disable UI
    }
    

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_targetPoint >= followPoints.Count) // if the array has reached the end 
        {
            _playerParent.SetActive(true); // enable player
            _canvas.SetActive(true); // enable UI
            _previewCamera.enabled = false; // disable preview camera
            Destroy(this.gameObject); // destroy self
        }
        var moveStep = moveSpeed * Time.deltaTime; // move speed calculation
        var rotateStep = rotateSpeed * Time.deltaTime; // rotate speed calculation
        _cameraObject.transform.position =
            Vector3.MoveTowards(_cameraObject.transform.position, followPoints[_targetPoint].transform.position, moveStep); // make camera move towards next point with the calculated move speed
        if (followPoints[_targetPoint].transform.localRotation != _cameraObject.transform.localRotation) // if next point has rotation
        {
            _cameraObject.transform.rotation = Quaternion.RotateTowards(_cameraObject.transform.localRotation,
                followPoints[_targetPoint].transform.localRotation, rotateStep); // rotate towards next point
        }

        if (!(Vector3.Distance(_cameraObject.transform.position, followPoints[_targetPoint].transform.position) <
              0.001f) ||
            _cameraObject.transform.localRotation != followPoints[_targetPoint].transform.localRotation) return; // return if not at next point, prevents nested if statements
        _targetPoint++; // set target point to next
    }
    
    
}
