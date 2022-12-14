using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ExtendingVector3
{
    public static bool IsGreaterOrEqual(this Vector3 local, Vector3 other)
    {
        if(local.x >= other.x && local.y >= other.y && local.z >= other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsLesserOrEqual(this Vector3 local, Vector3 other)
    {
        if(local.x <= other.x && local.y <= other.y && local.z <= other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public static Vector3 ClampMagnitude(this Vector3 local, float max, float min)
    {
        double sm = local.sqrMagnitude;
        if(sm > (double)max * (double)max) return local.normalized * max;
        else if(sm < (double)min * (double)min) return local.normalized * min;
        return local;
    }
}

public class renderWithinRadius : MonoBehaviour
{
    [SerializeField] private int postProcessingLayer;
    [SerializeField] private int noPostProcessingLayer;
    private Transform _sphereRadiusTransform;
    private GameObject _player;
    private playerController _playerController;
    private bool _isWithinSafeZone = false;
    private Vector3 _maxRadiusScale;
    private Vector3 _currentRadiusSize;
    private Vector3 _baseRadiusSize;
    private int _radiusSizeChangeRate;
    private int _currentScale;
    private int _radiusStartSize;
    private Vector3 _targetScale;
    public bool IsWithinSafeZone
    {
        get => _isWithinSafeZone;
        set => _isWithinSafeZone = value;
    }

    public int RadiusStartSize
    {
        get => _radiusStartSize;
        set => _radiusStartSize = value;
    }
    
    public Vector3 MaxRadiusSize
    {
        get => _maxRadiusScale;
        set => _maxRadiusScale = value;
    }

    public int RadiusSizeChangeRate
    {
        get => _radiusSizeChangeRate;
        set => _radiusSizeChangeRate = value;
    }
    
    private void Start()
    {
        _sphereRadiusTransform = gameObject.transform;
        _baseRadiusSize = _sphereRadiusTransform.localScale;
        transform.localScale = _baseRadiusSize * _radiusStartSize;
        _currentScale = _radiusStartSize;
        _targetScale = _baseRadiusSize * _radiusStartSize;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<playerController>();
    }
    
    private void Update()
    {
        // _targetScale = _targetScale.ClampMagnitude(_maxRadiusScale.x, 0f);
#pragma warning disable CS0642
        if (_targetScale.IsGreaterOrEqual(Vector3.zero));
#pragma warning restore CS0642
        {
            _sphereRadiusTransform.localScale = _targetScale;
        }

        switch (_isWithinSafeZone)
        {
            case true when _targetScale.IsLesserOrEqual(_maxRadiusScale):
                _targetScale += new Vector3(Time.deltaTime * _radiusSizeChangeRate, Time.deltaTime * _radiusSizeChangeRate,
                    Time.deltaTime * _radiusSizeChangeRate);
                break;
            case false when _targetScale.IsGreaterOrEqual(new Vector3(0,0,0)):
                _targetScale -= new Vector3(Time.deltaTime * _radiusSizeChangeRate, Time.deltaTime * _radiusSizeChangeRate,
                    Time.deltaTime * _radiusSizeChangeRate);
                break;
            default:
            {
                if (_currentScale == 0)
                {
                    _playerController.PlayerHealth = 0f;
                }

                break;
            }
        }
        
        
    }

    public void Init(int radiusSizeChangeRate, int radiusStartSize, Vector3 radiusMaxSize)
    {
        _radiusSizeChangeRate = radiusSizeChangeRate;
        _radiusStartSize = radiusStartSize;
        _maxRadiusScale = radiusMaxSize;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            other.gameObject.layer = noPostProcessingLayer;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            other.gameObject.layer = postProcessingLayer;
        }

    }
}