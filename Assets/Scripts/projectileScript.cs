using System;
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

    private GameObject _collidedEnemy;
    private mothController _collidedEnemyScript;
    
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
        StartCoroutine(DespawnCountdown());
        _projectileRigidbody = GetComponent<Rigidbody>();
        _projectileRigidbody.AddForce(transform.forward * _projectileSpeed, ForceMode.Impulse);
        transform.localScale = transform.localScale * _projectileCharge * 1.5f;
        // Debug.Log("Spawned projectile with a damage of " + _projectileDamage + " and a scale of " + transform.localScale);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with " + collision.gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _collidedEnemy = collision.gameObject;
            _collidedEnemyScript = _collidedEnemy.GetComponent<mothController>();
            _collidedEnemyScript.Damage(_projectileDamage);
            Despawn();
        }

    }

    private IEnumerator DespawnCountdown()
    {
        yield return new WaitForSeconds(_projectileDespawnRate);
        Despawn();
    }

    private void Despawn()
    {
        Destroy(this.gameObject);
    }
}
