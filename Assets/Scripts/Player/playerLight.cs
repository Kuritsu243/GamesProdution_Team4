using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLight : MonoBehaviour
{
    [SerializeField] private float minLightIntensity;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] private float lightDampeningRate;
    [SerializeField] private float lightStrength;
    [SerializeField] private bool enableLightFlicker;
    
    private Light _playerLight;
    private float _lightIntensity;
    private bool _isFlickering;

    private void Start()
    {
        _playerLight = GetComponentInChildren<Light>();
        _lightIntensity = _playerLight.intensity;
        StartCoroutine(FlickerLight());
    }

    private void Update()
    {
        if (enableLightFlicker && !_isFlickering)
        {
            StartCoroutine(FlickerLight());
        }
    }

    private IEnumerator FlickerLight()
    {
        _isFlickering = true;
        while (enableLightFlicker)
        {
            _playerLight.intensity = Mathf.Lerp(_playerLight.intensity,
                Random.Range(_lightIntensity - minLightIntensity, _lightIntensity + maxLightIntensity),
                lightStrength * Time.deltaTime);
            yield return new WaitForSeconds(lightDampeningRate);
        }
        _isFlickering = false;
    }
    
}
