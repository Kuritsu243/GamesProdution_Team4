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
    private bool _isCurrentlyPaused;
    // serialized fields that can be accessed in the inspector
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBackground;
    [SerializeField] private Image chargeBar;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // get player gameobject
        _playerHealth = _player.GetComponent<playerHealth>();
        _playerShooting =
            _player.GetComponent<playerShooting>(); // get player controller component from player gameobject
        pauseButton.onClick.AddListener(PauseButtonClicked);
        resumeButton.onClick.AddListener(ResumeButtonClicked);
        mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
        quitButton.onClick.AddListener(QuitButtonClicked);
        pauseScreen.SetActive(false);
    }
    void FixedUpdate()
    {
        healthBar.fillAmount = _playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth;
        // fills the health bar image by how much health the player has compared to their max health
        if (_playerShooting.IsCharging)
        {
            chargeBar.fillAmount = _playerShooting.projectileChargeDuration / _playerShooting.ProjectileMaxCharge;
        }
        else if (!_playerShooting.IsCharging)
        {
            chargeBar.fillAmount = 0;
        }
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
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            EditorApplication.Exit(0);
        }
#endif
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
        }
    }

    static void MainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
