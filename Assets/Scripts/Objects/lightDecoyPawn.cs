using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDecoyPawn : MonoBehaviour
{
    // Serialized variables
    [SerializeField] private Material inactiveDecoyMaterial;
    [SerializeField] private Material activeDecoyMaterial;
    [SerializeField] private int decoyActiveDuration;
    // private variables
    #pragma warning disable CS0219
    private bool _isLit = false;
    #pragma warning restore CS0219
    private Renderer _lightDecoyRenderer;

    private void Start()
    {
        _lightDecoyRenderer = GetComponent<Renderer>(); // get light renderer
    }

    private void DisableDecoy()
    {
        _lightDecoyRenderer.sharedMaterial = inactiveDecoyMaterial; // set decoy material to inactive material
        _isLit = false; // is lit is false
        gameObject.tag = "recentlyDisabledDecoy"; // set tag to recently disabled decoy
    }

    public void ActivateDecoy() // activate decoy
    {
        _lightDecoyRenderer.sharedMaterial = activeDecoyMaterial; // set material to lit material
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
