using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private int _currency = 200;
    private int _decoysPurchased;
    private int _healthUpgradesPurchased;
    private int _chargeSizeUpgradesPurchased;
    private int _damageUpgradesPurchased;
    private int _moveSpeedUpgradesPurchased;

    public int Currency { get => _currency; set => _currency = value; }
    public int DecoysPurchased { get => _decoysPurchased; set => _decoysPurchased = value; }
    public int HealthUpgradesPurchased { get => _healthUpgradesPurchased; set => _healthUpgradesPurchased = value; }
    public int ChargeSizeUpgradesPurchased { get => _chargeSizeUpgradesPurchased; set => _chargeSizeUpgradesPurchased = value; }
    public int DamageUpgradesPurchased { get => _damageUpgradesPurchased; set => _damageUpgradesPurchased = value; }
    public int MoveSpeedUpgradesPurchased { get => _moveSpeedUpgradesPurchased; set => _moveSpeedUpgradesPurchased = value; }
    private void Start()
    {
        Reset();
        PlayerPrefs.SetInt("currency", _currency);
        PlayerPrefs.Save();
        DontDestroyOnLoad(gameObject);
        Load();
    }

    private void FixedUpdate()
    {
        Load();
    }

    public void Load()
    {
        PlayerPrefs.GetInt("currency", _currency);
        PlayerPrefs.GetInt("decoysPurchased", _decoysPurchased);
        PlayerPrefs.GetInt("healthUpgradesPurchased", _healthUpgradesPurchased);
        PlayerPrefs.GetInt("chargeSizeUpgradesPurchased", _chargeSizeUpgradesPurchased);
        PlayerPrefs.GetInt("damageUpgradesPurchased", _damageUpgradesPurchased);
        PlayerPrefs.GetInt("moveSpeedUpgradesPurchased", _moveSpeedUpgradesPurchased);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("currency", _currency);
        PlayerPrefs.SetInt("decoysPurchased", _decoysPurchased);
        PlayerPrefs.SetInt("chargeSizeUpgradesPurchased", _chargeSizeUpgradesPurchased);
        PlayerPrefs.SetInt("damageUpgradesPurchased", _damageUpgradesPurchased);
        PlayerPrefs.SetInt("moveSpeedUpgradesPurchased", _moveSpeedUpgradesPurchased);
    }

    public void EndOrDeath()
    {
        PlayerPrefs.SetInt("currency", _currency);
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("currency", 0);
        PlayerPrefs.SetInt("decoysPurchased", 0);
        PlayerPrefs.SetInt("chargeSizeUpgradesPurchased", 0);
        PlayerPrefs.SetInt("damageUpgradesPurchased", 0);
        PlayerPrefs.SetInt("moveSpeedUpgradesPurchased", 0);
    }
}
