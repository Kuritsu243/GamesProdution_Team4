using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    private Vector3 _bezierPosition;
    private bool _movingAlongBezier;
    private bool _hasBezierMoveBeenCalled;
    private bool _movingToCurve;
    private bool _atStartOfCurve;
    private const string CurvePattern = @"\bCurve\b\([0]\)";
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
        _movingAlongBezier = false;
        _hasBezierMoveBeenCalled = false;
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
        MoveCamera(_targetPoint);
        Debug.Log(followPoints[_targetPoint].name);
        Debug.Log(followPoints[_targetPoint].name.Contains("Curve(0)"));

    }

    private void MoveCamera(int pointNumber)
    {
        var moveStep = moveSpeed * Time.deltaTime; // move speed calculation
        var rotateStep = rotateSpeed * Time.deltaTime; // rotate speed calculation

        _movingToCurve = CheckIfStartOfCurve(followPoints[pointNumber].name);
        
        if (_movingToCurve)
        {
            var destination = followPoints[pointNumber].transform.position;
            _cameraObject.transform.position = Vector3.MoveTowards(_cameraObject.transform.position,
                destination, moveStep);
            if (!(Vector3.Distance(_cameraObject.transform.position, destination) < 0.01)) return;
            _movingAlongBezier = true;
            _movingToCurve = false;
        }

        if (_movingAlongBezier && _movingToCurve)
        {
            return;
        }
        
        switch (_movingAlongBezier)
        {
            case true:
                MovingAlongBezier();
                break;
            case false:
                MovingAlongPoint(moveStep, rotateStep);
                break;
        }


    }

    private IEnumerator EndOfBezier(float timeTaken)
    {
        yield return new WaitForSeconds(timeTaken);
        _targetPoint += 4;
        _movingAlongBezier = false;
        _hasBezierMoveBeenCalled = false;
    }

    private void MovingAlongBezier()
    {
        var point1 = followPoints[_targetPoint].transform.position;
        var point2 = followPoints[_targetPoint+1].transform.position;
        var point3 = followPoints[_targetPoint+2].transform.position;
        var point4 = followPoints[_targetPoint+3].transform.position;
        var Points = new Vector3[4] {point1, point3, point2, point4};

        if (_hasBezierMoveBeenCalled) return;
        LeanTween.move(_cameraObject, new LTBezierPath(Points), moveSpeed);
        StartCoroutine(EndOfBezier(moveSpeed));
        _hasBezierMoveBeenCalled = true;
    }

    private void MovingAlongPoint(float moveSpeed, float rotateSpeed)
    {

        if (_movingAlongBezier) return;
        _cameraObject.transform.position = Vector3.MoveTowards(_cameraObject.transform.position,
            followPoints[_targetPoint].transform.position, moveSpeed);
        if (followPoints[_targetPoint].transform.localRotation != _cameraObject.transform.localRotation)
        {
            _cameraObject.transform.rotation = Quaternion.RotateTowards(_cameraObject.transform.localRotation,
                followPoints[_targetPoint].transform.localRotation, rotateSpeed);
        }
        
        if ((Vector3.Distance(_cameraObject.transform.position, followPoints[_targetPoint].transform.position) <
                0.01f && !_movingAlongBezier|| _cameraObject.transform.localRotation != followPoints[_targetPoint].transform.localRotation) && !_movingAlongBezier)
        {
            _targetPoint++;
        }

    }
    
    private static bool CheckIfStartOfCurve(string pointName)
    {
        return Regex.Match(pointName, CurvePattern).Value == "Curve(0)";
    }
    
    }
