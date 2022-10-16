using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class gameSaturationModifier : MonoBehaviour
{
    private ColorAdjustments _colorGrading;
    private float _enemiesCurrentlyInScene;

    // private variables
    private float _enemiesInSceneCount;
    private GameObject _postProcessController;
    private Volume _postProcessVolume;
    private float _saturation;
    private playerHealth _playerHealth;

    // encapsulated fields
    public float EnemiesCurrentlyInScene
    {
        get => _enemiesCurrentlyInScene;
        set => _enemiesCurrentlyInScene = value;
    }

    void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<playerHealth>();
        _postProcessController = GameObject.FindGameObjectWithTag("postProcessingController"); // get post processing controller
        _postProcessVolume = _postProcessController.GetComponent<Volume>(); // get post process volume component
        _postProcessVolume.profile.TryGet(out _colorGrading); // apply colour grading settings
        GetAllEnemiesInScene(); // gets count of enemies
        CalculateSaturationLevel(); // set saturation level

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     CalculateSaturationLevel();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     SetSaturationLevel();
        // }
        Debug.Log(_saturation);
    }

    void GetAllEnemiesInScene() // get count of enemies
    {
        _enemiesInSceneCount = GameObject.FindGameObjectsWithTag("Enemy").Length; // creates array of all enemies and counts how many exist
        _enemiesCurrentlyInScene = _enemiesInSceneCount; // sets currently in scene to size of array above
    }

    public void CalculateSaturationLevel() // calculates saturation level
    {
        if (_playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth >= 0.99)
        {
            _saturation = 0;
        }
        else
        {
            _saturation = -100 + ((_playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth) * 100); // calculate percentage to scale with player health
        }

        SetSaturationLevel();
    }

    void SetSaturationLevel() // set saturation level
    {
        _colorGrading.saturation.value = (int)(_saturation); // set value to saturation variable after being converted to int from float
        _postProcessVolume.profile.TryGet(out _colorGrading); // apply saturation value
    }
}
