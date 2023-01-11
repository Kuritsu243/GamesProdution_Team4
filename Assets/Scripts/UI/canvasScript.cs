using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class canvasScript : MonoBehaviour
{
    private GameObject _player;
    private playerHealth _playerHealth;
    private playerShooting _playerShooting;
    private DontDestroy _dontDestroy;
    private Vector2 _healthBarOriginalPos;
    private RectTransform _healthBarFlameRect;
    private bool _hasFaded;
    private Animator _crossFade;
    private GameObject _crossFadeObj;

#pragma warning disable CS0414
    private bool _isCurrentlyPaused = false;
#pragma warning restore CS0414
    public int lightDecoyInv;
    // serialized fields that can be accessed in the inspector
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarFlame;
    [SerializeField] private Image chargeBar;
    [SerializeField] private Image chargeBarBG;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject lightDecoyPrefab;
    [SerializeField] private Button lightDecoyButton;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player gameobject
        _playerHealth = _player.GetComponent<playerHealth>();
        _playerShooting =
            _player.GetComponent<playerShooting>(); // get player controller component from player gameobject
        _dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();

        lightDecoyInv = _dontDestroy.DecoysPurchased;
        pauseScreen.SetActive(false);
        _healthBarOriginalPos = healthBar.rectTransform.rect.position;
        _healthBarFlameRect = healthBarFlame.GetComponent<RectTransform>();
        healthBarFlame.enabled = false;
        _crossFadeObj = GameObject.FindGameObjectWithTag("crossFade");
        _crossFade = _crossFadeObj.GetComponent<Animator>();

        StartCoroutine(Transition());
        
        // button listeners
        pauseButton.onClick.AddListener(PauseButtonClicked);
        lightDecoyButton.onClick.AddListener(lightDecoyButtonClicked);
        resumeButton.onClick.AddListener(ResumeButtonClicked);
        mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
        quitButton.onClick.AddListener(QuitButtonClicked);

    }



    private void FixedUpdate()
    {
        healthBar.fillAmount = _playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth;
        if (healthBar.fillAmount < 1)
        {
            healthBarFlame.enabled = true;
            _healthBarFlameRect.anchorMin = new Vector2(healthBar.fillAmount, _healthBarFlameRect.anchorMin.y);
            _healthBarFlameRect.anchorMax = new Vector2(healthBar.fillAmount, _healthBarFlameRect.anchorMax.y);
            _healthBarFlameRect.anchoredPosition = Vector2.zero;
        }

        if (_playerShooting.IsCharging)
        {
            chargeBar.fillAmount = _playerShooting.projectileChargeDuration / _playerShooting.ProjectileMaxCharge;
            chargeBarBG.enabled = true;
        }
        else
        {
            chargeBar.fillAmount = 0;
            chargeBarBG.enabled = false;
        }
        // chargeBar.fillAmount = _playerShooting.IsCharging switch
        // {
        //     // fills the health bar image by how much health the player has compared to their max health
        //     true => _playerShooting.projectileChargeDuration / _playerShooting.ProjectileMaxCharge,
        //     false => 0
        // };
    }

    private void PauseButtonClicked()
    {
        _isCurrentlyPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;

    }

    private void ResumeButtonClicked()
    {
        _isCurrentlyPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private static void QuitButtonClicked()
    {
#if UNITY_EDITOR
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                EditorApplication.Exit(0);
                break;
            case RuntimePlatform.Android:
            case RuntimePlatform.WindowsPlayer:
                Application.Quit();
                break;
        }
#endif
    }

    private static void MainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator Transition()
    {
        yield return new WaitForSeconds(1f);
        _crossFadeObj.SetActive(false);
        
    }

    private void lightDecoyButtonClicked()
    {
        if (lightDecoyInv < 1) return;
        lightDecoyInv--;
        var newDecoy = Instantiate(lightDecoyPrefab, _player.transform.position, _player.transform.rotation);
        newDecoy.GetComponent<lightDecoyPawn>().ActivateDecoy();
    }
  



}
