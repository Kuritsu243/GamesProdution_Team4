using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerController2 : MonoBehaviour
{
    [SerializeField] private float cameraHeight;
    [SerializeField] private float cameraZOffset;
    private inputSystem _inputSystem;
    private playerMovement _playerMovement;
    private cameraFollow _cameraFollow;
    private GameObject _player;
    private Camera _playerCamera;
    private GameObject _indicatorTextParent;
    private TextMeshProUGUI _topText;
    private TextMeshProUGUI _bottomText;

    public float CameraZOffset
    {
        get => cameraZOffset;
        set => cameraZOffset = value;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        _player = gameObject;
        _inputSystem = GetComponent<inputSystem>();
        _playerMovement = GetComponent<playerMovement>();
        _cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraFollow>();
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _indicatorTextParent = GameObject.FindGameObjectWithTag("indicatorText");
        _topText = GameObject.FindGameObjectWithTag("topIndicator").GetComponent<TextMeshProUGUI>();
        _bottomText = GameObject.FindGameObjectWithTag("bottomIndicator").GetComponent<TextMeshProUGUI>();

    }
    

    // Update is called once per frame
    public void FixedUpdate()
    {
        _inputSystem.HandleAllInputs();
        _playerMovement.HandleAllMovement();
        _cameraFollow.MoveCamera(cameraHeight,cameraZOffset, _player.transform.position);
    }

    // private void OnEnable()
    // {
    //     _topText.text = "";
    //     _bottomText.text = "Left stick - move      Right stick - look / shoot";
    //     StartCoroutine(DisableText());
    // }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(2);
        _bottomText.text = "";
        _indicatorTextParent.SetActive(false);
    }

    public void EnableTextAtStart()
    {
        _topText.text = "";
        _bottomText.text = "Left stick - move      Right stick - look / shoot";
        StartCoroutine(DisableText());
    }
}
