using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class projectileScript : MonoBehaviour
{
    private float _projectileSpeed;
    private float _projectileDamage;
    private float _projectileDespawnRate;
    private float _projectileCharge;
    private Rigidbody _projectileRigidbody;
    
    
    public void Init(float projectileSpeed, float projectileDamage, float projectileDespawnRate, float projectileCharge)
    {
        _projectileSpeed = projectileSpeed;
        _projectileDamage = projectileDamage;
        _projectileDespawnRate = projectileDespawnRate;
        _projectileCharge = projectileCharge;
    }
    // Start is called before the first frame update
    void Start()
    {
        _projectileRigidbody = GetComponent<Rigidbody>();
        _projectileRigidbody.AddForce(transform.forward * _projectileSpeed, ForceMode.Impulse);
        transform.localScale = transform.localScale * _projectileCharge * 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
