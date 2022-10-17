using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class winZoneScript : MonoBehaviour
{
    private GameObject _collidedPlayer;
    public bool haveConditionsBeenMet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && haveConditionsBeenMet)
        {
            _collidedPlayer = other.gameObject;
            SceneManager.LoadScene("Win_Screen");
        }
    }
}
