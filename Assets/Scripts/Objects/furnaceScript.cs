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
#pragma warning disable CS0414
    private bool _isLit;
#pragma warning restore CS0414
    private GameObject _player;
    private float _distanceToPlayer;
    private float _currentLightProgress;
    private float _currentProgressionPercentage;
    private Transform _playerTransform;
    private playerHealth _playerHealth;
    private GameObject _furnaceUITextParent;
    private Image _furnaceBarImage;
    private Image _furnaceBarImageBG;
    private TextMeshProUGUI _furnaceBarText;
    private winZoneScript _winZoneScript;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player
        _playerTransform = _player.transform; // get player transform
        _furnaceUITextParent = GameObject.FindGameObjectWithTag("furnaceText"); // get furnace canvas text
        _furnaceBarImage = GameObject.FindGameObjectWithTag("furnaceFillBar").GetComponent<Image>(); // get furnace progress bar
        _furnaceBarImageBG = GameObject.FindGameObjectWithTag("furnaceFillBarBG").GetComponent<Image>(); // get furnace progress bar bg
        _furnaceBarText = _furnaceUITextParent.GetComponentInChildren<TextMeshProUGUI>(); // get bar text
        _winZoneScript = GameObject.FindGameObjectWithTag("winZone").GetComponent<winZoneScript>(); // get winzone script
        ToggleUIAssets(false); // disable UI assets
    }
    
    private void FixedUpdate()
    {
        if (NearPlayer() && !_winZoneScript.haveConditionsBeenMet)
        {
            ToggleUIAssets(true); // enable UI assets
        }
        // Debug.Log(_currentLightProgress);
        _furnaceBarImage.fillAmount = _currentLightProgress / lightCost; // adjust bar on how filled the furnace is
    }

    private void LightFurnace() // light furnace
    {
        _isLit = true; 
        _winZoneScript.haveConditionsBeenMet = true; // enable win zone
        _furnaceBarImage.enabled = false;
        _furnaceBarImageBG.enabled = false;
        _furnaceBarText.text = "Furnace lit! Go to the window to complete the level";
    }

    private bool NearPlayer()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        return _distanceToPlayer < interactableRadius; // return y/n if player is within the radius of furnace
    }

    private void ToggleUIAssets(bool yn) // toggle ui assets script
    {
        _furnaceBarImage.enabled = yn;
        _furnaceBarImageBG.enabled = yn;
        _furnaceBarText.enabled = yn;
    }

    public void AddProgress(float projectileAmount) // add progress to lighting the furnace
    {
        _currentLightProgress += projectileAmount; 
        _currentProgressionPercentage = _currentLightProgress / lightCost;
        if (_currentProgressionPercentage > 0.99f)
        {
            LightFurnace(); // if progression is full, light the furnace
        }
    }
    
}
