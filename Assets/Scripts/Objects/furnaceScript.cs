using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class furnaceScript : MonoBehaviour
{
    [SerializeField] private float interactableRadius;
    [SerializeField] private float lightCost;
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
    // Start is called before the first frame update
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
        _furnaceUITextParent = GameObject.FindGameObjectWithTag("furnaceText");
        _furnaceBarImage = _furnaceUITextParent.GetComponentInChildren<Image>();
        _furnaceBarText = _furnaceUITextParent.GetComponentInChildren<TextMeshProUGUI>();
        _winZoneScript = GameObject.FindGameObjectWithTag("winZone").GetComponent<winZoneScript>();
        ToggleUIAssets(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (NearPlayer())
        {
            Debug.Log("near player");
            ToggleUIAssets(true);
        }

        if (_furnaceUITextParent.activeSelf)
        {
            _furnaceBarImage.fillAmount = _currentProgressionPercentage;
        }
    }

    private void LightFurnace()
    {
        _isLit = true;
        _winZoneScript.haveConditionsBeenMet = true;
    }

    private bool NearPlayer()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        return _distanceToPlayer < interactableRadius;
    }

    private void ToggleUIAssets(bool yn)
    {
        _furnaceBarImage.enabled = yn;
        _furnaceBarText.enabled = yn;
    }

    public void AddProgress(float projectileAmount)
    {
        _currentLightProgression += projectileAmount;
        _currentProgressionPercentage = _currentLightProgression / lightCost;
        if (_currentProgressionPercentage > 0.99f)
        {
            LightFurnace();
        }
    }
    
}
