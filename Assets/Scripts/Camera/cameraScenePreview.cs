using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScenePreview : MonoBehaviour
{
    [SerializeField] List<GameObject> followPoints = new List<GameObject>();
    [SerializeField] private bool isMoving;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private GameObject _player;
    private GameObject _cameraObject;
    private Camera _previewCamera;
    private Camera _playerCamera;
    private Transform _targetPosition;
    private Transform _currentPosition;
    private int _targetPoint;
    private int _numberOfPoints;

    public List<GameObject> FollowPoints
    {
        get => followPoints;
        set => followPoints = value;
    }
    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _previewCamera = GetComponentInChildren<Camera>();
        _cameraObject = _previewCamera.gameObject;
        _playerCamera = _player.GetComponentInChildren<Camera>();
        _numberOfPoints = followPoints.Count;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var moveStep = moveSpeed * Time.deltaTime;
        var rotateStep = rotateSpeed * Time.deltaTime;
        _cameraObject.transform.position =
            Vector3.MoveTowards(_cameraObject.transform.position, followPoints[_targetPoint].transform.position, moveStep);
        if (followPoints[_targetPoint].transform.localRotation != _cameraObject.transform.localRotation)
        {
            _cameraObject.transform.rotation = Quaternion.RotateTowards(_cameraObject.transform.localRotation,
                followPoints[_targetPoint].transform.localRotation, rotateStep);
        }
        if (Vector3.Distance(_cameraObject.transform.position, followPoints[_targetPoint].transform.position) < 0.001f && _cameraObject.transform.localRotation == followPoints[_targetPoint].transform.localRotation)
        {
            _targetPoint++;
        }
    }
    
    
}
