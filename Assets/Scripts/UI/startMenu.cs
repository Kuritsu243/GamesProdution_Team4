using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class startMenu : MonoBehaviour
{

    private Animator _crossFade;
    private Button _startButton;
    private Button _exitButton;
    private bool _hasFaded;
    private DontDestroy _dontDestroy;
    private int _playerCurrency;
    private bool _isShopOpen;

    [SerializeField] private bool _isLoseScreen;
    // shop menu 
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private int upgradeCost;
    [SerializeField] private Button shopMenuButton;
    [SerializeField] private Button healthButton;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button moveSpeedButton;
    [SerializeField] private Button chargeSizeButton;
    [SerializeField] private Button buyDecoyButton;
    [SerializeField] private Button closeShopMenuButton;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI healthUpgradesPurchased;
    [SerializeField] private TextMeshProUGUI damageUpgradesPurchased;
    [SerializeField] private TextMeshProUGUI moveSpeedUpgradesPurchased;
    [SerializeField] private TextMeshProUGUI chargeSizeUpgradesPurchased;
    [SerializeField] private TextMeshProUGUI decoysPurchased;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _crossFade = GameObject.FindGameObjectWithTag("crossFade").GetComponent<Animator>();
        if (!_isLoseScreen)
        {
            _exitButton = GameObject.FindGameObjectWithTag("exitButton").GetComponent<Button>();
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        _dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
        _startButton = GameObject.FindGameObjectWithTag("startButton").GetComponent<Button>();
        _startButton.onClick.AddListener(OnStartButtonClicked);
        shopMenuButton.onClick.AddListener(ShopMenuButtonClicked);
        closeShopMenuButton.onClick.AddListener(CloseShopMenuButtonClicked);
        healthButton.onClick.AddListener(HealthButtonClicked);
        damageButton.onClick.AddListener(DamageButtonClicked);
        moveSpeedButton.onClick.AddListener(MoveSpeedButtonClicked);
        chargeSizeButton.onClick.AddListener(ChargeSizeButtonClicked);
        buyDecoyButton.onClick.AddListener(BuyDecoyButtonClicked);
        _playerCurrency = _dontDestroy.Currency;
        
        shopMenu.SetActive(false);

#if UNITY_ANDROID
        Application.targetFrameRate = 30;
#endif
    }



    private void FixedUpdate()
    {
        currencyText.text = "Owned: " + _dontDestroy.Currency;
        decoysPurchased.text = "Owned: " + _dontDestroy.DecoysPurchased;
        healthUpgradesPurchased.text = "Owned: " + _dontDestroy.HealthUpgradesPurchased;
        damageUpgradesPurchased.text = "Owned: " + _dontDestroy.DamageUpgradesPurchased;
        moveSpeedUpgradesPurchased.text = "Owned: " + _dontDestroy.MoveSpeedUpgradesPurchased;
        chargeSizeUpgradesPurchased.text = "Owned: " + _dontDestroy.ChargeSizeUpgradesPurchased;
        currencyText.text = "Money: " + _dontDestroy.Currency;
        
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
    
      
    private void BuyDecoyButtonClicked()
    {
        if (_playerCurrency < 0 || _playerCurrency < upgradeCost) return;
        _dontDestroy.Currency -= upgradeCost;
        _dontDestroy.DecoysPurchased++;
        _dontDestroy.Save();
        _dontDestroy.Load();
    }

    private void ChargeSizeButtonClicked()
    {
        if (_playerCurrency < 0 || _playerCurrency < upgradeCost) return;
        _dontDestroy.Currency -= upgradeCost;
        _dontDestroy.ChargeSizeUpgradesPurchased++;
        _dontDestroy.Save();
        _dontDestroy.Load();
    }

    private void MoveSpeedButtonClicked()
    {
        if (_playerCurrency < 0 || _playerCurrency < upgradeCost) return;
        _dontDestroy.Currency -= upgradeCost;
        _dontDestroy.MoveSpeedUpgradesPurchased++;
        _dontDestroy.Save();
        _dontDestroy.Load();
    }

    private void DamageButtonClicked()
    {
        if (_playerCurrency < 0 || _playerCurrency < upgradeCost) return;
        _dontDestroy.Currency -= upgradeCost;
        _dontDestroy.DamageUpgradesPurchased++;
        _dontDestroy.Save();
        _dontDestroy.Load();
    }

    private void HealthButtonClicked()
    {
        if (_playerCurrency < 0 || _playerCurrency < upgradeCost) return;
        _dontDestroy.Currency -= upgradeCost;
        _dontDestroy.HealthUpgradesPurchased++;
        _dontDestroy.Save();
        _dontDestroy.Load();
    }

    private void ShopMenuButtonClicked()
    {
        _isShopOpen = true;
        shopMenu.SetActive(true);
    }
    
    private void CloseShopMenuButtonClicked()
    {
        _isShopOpen = false;
        shopMenu.SetActive(false);
    }
}
