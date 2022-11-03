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
    // Start is called before the first frame update

    private void Awake()
    {
        _startButton = GameObject.FindGameObjectWithTag("startButton").GetComponent<Button>();
        _exitButton = GameObject.FindGameObjectWithTag("exitButton").GetComponent<Button>();
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
#if UNITY_ANDROID
        Application.targetFrameRate = 30;
#endif
    }

    // Update is called once per frame

    void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Greybox");  
    }

    void OnExitButtonClicked()
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
}
