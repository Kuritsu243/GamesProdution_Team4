using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Vignette = UnityEngine.Rendering.Universal.Vignette;


public class gameSaturationModifier : MonoBehaviour
{
    
    // private variables
    private ColorAdjustments _colorGrading;
    private Vignette _vignette;
    private GameObject _postProcessController;
    private Volume _postProcessVolume;
    private float _saturation;
    private float _vignetteStrength;
    private playerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<playerHealth>();
        _postProcessController = GameObject.FindGameObjectWithTag("postProcessingController"); // get post processing controller
        _postProcessVolume = _postProcessController.GetComponent<Volume>(); // get post process volume component
        _postProcessVolume.profile.TryGet(out _colorGrading); // apply colour grading settings
        _postProcessVolume.profile.TryGet(out _vignette);
        CalculateSaturationLevel(); // set saturation level
        CalculateVignetteStrength();

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

    public void CalculateVignetteStrength()
    {
        _vignetteStrength = 1 - _playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth;
        _vignette.intensity.value = _vignetteStrength;
        _postProcessVolume.profile.TryGet<Vignette>(out _vignette);


    }
    
    private void SetSaturationLevel() // set saturation level
    {
        _colorGrading.saturation.value = (int)(_saturation); // set value to saturation variable after being converted to int from float
        _postProcessVolume.profile.TryGet<ColorAdjustments>(out _colorGrading); // apply saturation value
    }
}
