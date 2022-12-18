using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class playerLight : MonoBehaviour
{
    // flicker script from: https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969
    [SerializeField] private float minLightIntensity;
    [SerializeField] private float maxLightIntensity;
    [Range(0,50)][SerializeField] private int lightSmoothing;
    [SerializeField] private bool enableLightFlicker;
    [SerializeField] private bool isPlayer;
    
    private Light _light;
    private Queue<float> _lightQueue;
    private float _lastSum = 0f;

    private void Start()
    {
        _light = isPlayer ? GameObject.FindGameObjectWithTag("playerLight").GetComponentInChildren<Light>() : GetComponentInChildren<Light>();

        if (enableLightFlicker)
        {
            _lightQueue = new Queue<float>(lightSmoothing);
        }
    }

    private void FixedUpdate()
    {
        if (!enableLightFlicker) return;
        while (_lightQueue.Count >= lightSmoothing)
        {
            _lastSum -= _lightQueue.Dequeue();
        }

        var newVal = Random.Range(minLightIntensity, maxLightIntensity);
        _lightQueue.Enqueue(newVal);
        _lastSum += newVal;

        _light.intensity = _lastSum / (float)_lightQueue.Count;

    }

    private void Reset()
    {
        _lightQueue.Clear();
        _lastSum = 0f;
    }
}
