using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{

    private Animator _crossFade;
    private Button _startButton;
    private Button _exitButton;
    private bool _hasFaded;

    [SerializeField] private bool _isLoseScreen;
    // Start is called before the first frame update

    private void Awake()
    {
        _crossFade = GameObject.FindGameObjectWithTag("crossFade").GetComponent<Animator>();
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

    private void FixedUpdate()
    {
        if (_hasFaded) return;
        if (_crossFade.gameObject.GetComponent<Image>().color.a != 0) return;
        _crossFade.gameObject.SetActive(false);
        _hasFaded = true;
    }

    // Update is called once per frame

    private void OnStartButtonClicked()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadStartScene());  
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
            default:
                throw new ArgumentOutOfRangeException();
        }
#endif
    }
    
    private IEnumerator LoadStartScene()
    {            
        _crossFade.gameObject.SetActive(true);
        _crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("Greybox");
    }
    
    private IEnumerator LoadDeathScene()
    {
        _crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lose_Screen");
    }
}
