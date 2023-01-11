using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDecoyPawn : MonoBehaviour
{
    // Serialized variables
    [SerializeField] private int decoyActiveDuration;
    // private variables
#pragma warning disable CS0219
    private bool _isLit = false;
    private Light _decoyLight;
#pragma warning restore CS0414
    
    private void DisableDecoy()
    {
        _decoyLight.enabled = false;
        _isLit = false; // is lit is false
        gameObject.tag = "recentlyDisabledDecoy"; // set tag to recently disabled decoy
        Destroy(this.gameObject);
    }

    public void ActivateDecoy() // activate decoy
    {
        _decoyLight = GetComponentInChildren<Light>();
        _decoyLight.enabled = true;
        StartCoroutine(DecoyActive(decoyActiveDuration)); // start countdown for duration it's enabled
        _isLit = true; // is lit is true
        gameObject.tag = "activeDecoy"; // change tag to indicate it is active
    }

    private IEnumerator DecoyActive(float decoyLitDuration) // decoy countdown
    {
        gameObject.layer = 12;
        yield return new WaitForSeconds(decoyLitDuration); // wait for duration decoy is lit for
        DisableDecoy(); // disable decoy
        gameObject.layer = 6;

    }
}
//
// _decoyInRange.GetComponent<Renderer>().material = litMaterial;
// _isLit = true;
// StartCoroutine(DecoyActive());
