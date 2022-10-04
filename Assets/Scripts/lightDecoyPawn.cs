using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDecoyPawn : MonoBehaviour
{

    private bool _isLit = false;
    private Renderer _lightDecoyRenderer;
    [SerializeField] private Material inactiveDecoyMaterial;
    [SerializeField] private Material activeDecoyMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _lightDecoyRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DisableDecoy()
    {
        _lightDecoyRenderer.material = inactiveDecoyMaterial;
        _isLit = false;
        gameObject.tag = "recentlyDisabledDecoy";
    }

    public void ActivateDecoy(float duration)
    {
        _lightDecoyRenderer.material = activeDecoyMaterial;
        StartCoroutine(DecoyActive(duration));
        _isLit = true;
        gameObject.tag = "activeDecoy";
    }

    private IEnumerator DecoyActive(float decoyLitDuration)
    {
        yield return new WaitForSeconds(decoyLitDuration);
        DisableDecoy();
        
    }
}
//
// _decoyInRange.GetComponent<Renderer>().material = litMaterial;
// _isLit = true;
// StartCoroutine(DecoyActive());
