using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float playerMaxHealth;
    private float _playerHealth;
    private gameSaturationModifier _gameSaturationModifier;
    private Animator _crossFade;
    private bool _hasSceneChangeBeenCalled;

    public float PlayerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }
    
    public float PlayerMaxHealth => playerMaxHealth;

    private void Start()
    {
        _playerHealth = playerMaxHealth;
        _gameSaturationModifier =
            GameObject.FindGameObjectWithTag("eventManager").GetComponent<gameSaturationModifier>();
        _crossFade = GameObject.FindGameObjectWithTag("crossFade").GetComponent<Animator>();
    }

    public void Damage(float damageAmount) // damage function
    {
        _playerHealth -= damageAmount; // reduces health by damage value passed through 
        _gameSaturationModifier.CalculateSaturationLevel();
        _gameSaturationModifier.CalculateVignetteStrength();
    }

    private void FixedUpdate()
    {
        if (_playerHealth <= 0.01f)
        {
            Die(); // kill player
        }
    }

    private void Die()
    { // destroys player gameobject
        if (_hasSceneChangeBeenCalled) return;
        StartCoroutine(LoadDeathScene());
    }
    
    private IEnumerator LoadDeathScene()
    {
        _crossFade.gameObject.SetActive(true);
        _crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("Lose_Screen");
        _hasSceneChangeBeenCalled = true;
    }
}
