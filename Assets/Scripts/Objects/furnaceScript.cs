using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class furnaceScript : MonoBehaviour
{
    // vars accessible in editor
    [SerializeField] private float interactableRadius;
    [SerializeField] private float lightCost;
    // private vars
    private bool _isLit;
    private GameObject _player;
    private float _distanceToPlayer;
    private float _currentLightProgression = 0f;
    private float _currentProgressionPercentage;
    private Transform _playerTransform;
    private playerHealth _playerHealth;
    private GameObject _furnaceUITextParent;
    private Image _furnaceBarImage;
    private TextMeshProUGUI _furnaceBarText;
    private winZoneScript _winZoneScript;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player
        _playerTransform = _player.transform; // get player transform
        _furnaceUITextParent = GameObject.FindGameObjectWithTag("furnaceText"); // get furnace canvas text
        _furnaceBarImage = _furnaceUITextParent.GetComponentInChildren<Image>(); // get furnace progress bar
        _furnaceBarText = _furnaceUITextParent.GetComponentInChildren<TextMeshProUGUI>(); // get bar text
        _winZoneScript = GameObject.FindGameObjectWithTag("winZone").GetComponent<winZoneScript>(); // get winzone script
        ToggleUIAssets(false); // disable UI assets
    }
    
    private void FixedUpdate()
    {
        if (NearPlayer())
        {
            Debug.Log("near player");
            ToggleUIAssets(true); // enable UI assets
        }

        if (_furnaceUITextParent.activeSelf) // if text is enabled
        {
            _furnaceBarImage.fillAmount = _currentProgressionPercentage; // adjust bar on how filled the furnace is
        }
    }

    private void LightFurnace() // light furnace
    {
        _isLit = true; 
        _winZoneScript.haveConditionsBeenMet = true; // enable win zone
    }

    private bool NearPlayer()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        return _distanceToPlayer < interactableRadius; // return y/n if player is within the radius of furnace
    }

    private void ToggleUIAssets(bool yn) // toggle ui assets script
    {
        _furnaceBarImage.enabled = yn;
        _furnaceBarText.enabled = yn;
    }

    public void AddProgress(float projectileAmount) // add progress to lighting the furnace
    {
        _currentLightProgression += projectileAmount; 
        _currentProgressionPercentage = _currentLightProgression / lightCost;
        if (_currentProgressionPercentage > 0.99f)
        {
            LightFurnace(); // if progression is full, light the furnace
        }
    }
    
}
