using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScore : MonoBehaviour
{
    private int _playerScore;
    private DontDestroy _dontDestroy;

    public int PlayerScore { get => _playerScore; set => _playerScore = value; }
    private void Start()
    {
        _dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroy>();
    }

    public void AddToScore(int amount)
    {
        _playerScore += amount;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("currency", _dontDestroy.Currency + _playerScore);
        PlayerPrefs.SetInt("playerScore", _playerScore);
        PlayerPrefs.Save();
    }

}
