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
    
    
    public void Init(float projectileSpeed, float projectileDamage, float projectileDespawnRate)
    {
        _projectileSpeed = projectileSpeed;
        _projectileDamage = projectileDamage;
        _projectileDespawnRate = projectileDespawnRate;
    }
    // Start is called before the first frame update
    void Start()
    {
        _projectileRigidbody = GetComponent<Rigidbody>();
        _projectileRigidbody.AddForce(transform.forward * _projectileSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
