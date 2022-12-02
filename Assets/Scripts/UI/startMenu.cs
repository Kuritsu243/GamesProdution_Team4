using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{

    private Button _startButton;
    private Button _exitButton;

    [SerializeField] private bool _isLoseScreen;
    // Start is called before the first frame update

    private void Awake()
    {
        if (!_isLoseScreen)
        {
            _exitButton = GameObject.FindGameObjectWithTag("exitButton").GetComponent<Button>();
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        _startButton = GameObject.FindGameObjectWithTag("startButton").GetComponent<Button>();
        _startButton.onClick.AddListener(OnStartButtonClicked);

#if UNITY_ANDROID
        Application.targetFrameRate = 30;
#endif
    }

    // Update is called once per frame

    private static void OnStartButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Greybox");  
    }

    private static void OnExitButtonClicked()
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
}
