using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDecoyPawn : MonoBehaviour
{
    // Serialized variables
    [SerializeField] private Material inactiveDecoyMaterial;
    [SerializeField] private Material activeDecoyMaterial;

    // private variables
    private bool _isLit = false;
    private Renderer _lightDecoyRenderer;

    void Start()
    {
        _lightDecoyRenderer = GetComponent<Renderer>(); // get light renderer
    }

    public void DisableDecoy()
    {
        _lightDecoyRenderer.material = inactiveDecoyMaterial; // set decoy material to inactive material
        _isLit = false; // is lit is false
        gameObject.tag = "recentlyDisabledDecoy"; // set tag to recently disabled decoy
    }

    public void ActivateDecoy(float duration) // activate decoy
    {
        _lightDecoyRenderer.material = activeDecoyMaterial; // set material to lit material
        StartCoroutine(DecoyActive(duration)); // start countdown for duration it's enabled
        _isLit = true; // is lit is true
        gameObject.tag = "activeDecoy"; // change tag to indicate it is active
    }

    private IEnumerator DecoyActive(float decoyLitDuration) // decoy countdown
    {
        yield return new WaitForSeconds(decoyLitDuration); // wait for duration decoy is lit for
        DisableDecoy(); // disable decoy
        
    }
}
//
// _decoyInRange.GetComponent<Renderer>().material = litMaterial;
// _isLit = true;
// StartCoroutine(DecoyActive());
