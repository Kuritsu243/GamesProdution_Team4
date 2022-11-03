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
        Debug.Log(other.gameObject.tag);
        if ((!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("playerCapsule") &&
             !other.gameObject.CompareTag("lightRadius")) || !haveConditionsBeenMet) return;
        _collidedPlayer = other.gameObject;
        SceneManager.LoadScene("Win_Screen");

    }
}
