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
    private void Start()
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

    private void FixedUpdate()
    {
        healthBar.fillAmount = _playerHealth.PlayerHealth / _playerHealth.PlayerMaxHealth;
        chargeBar.fillAmount = _playerShooting.IsCharging switch
        {
            // fills the health bar image by how much health the player has compared to their max health
            true => _playerShooting.projectileChargeDuration / _playerShooting.ProjectileMaxCharge,
            false => 0
        };
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
}
