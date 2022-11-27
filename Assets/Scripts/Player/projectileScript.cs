using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class projectileScript : MonoBehaviour
{
    // collision variables
    private GameObject _collidedEnemy;
    private GameObject _collidedDecoy;
    private GameObject _collidedFurnace;
    private GameObject _projectileParent;
    private lightDecoyPawn _decoyScript;
    private mothController _collidedEnemyScript;
    private furnaceScript _collidedFurnaceScript;
    private float _projectileCharge;
    private float _projectileDamage;
    private float _projectileDespawnRate;
    private Rigidbody _projectileRigidbody;
    private SpriteRenderer _projectileSprite;
    private GameObject _projectileSpriteGameObject;
    private Camera _playerCamera;

    // projectile variables
    private float _projectileSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DespawnCountdown()); // start countdown until despawn
        _projectileRigidbody = GetComponent<Rigidbody>(); // get rigidbody of the projectile
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _projectileSprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
        _projectileSpriteGameObject = _projectileSprite.gameObject;
        _projectileSpriteGameObject.transform.LookAt(transform.forward);
        _projectileParent = transform.parent.gameObject;
        
        _projectileRigidbody.AddForce(transform.forward * _projectileSpeed, ForceMode.Impulse); // add force to travel in the intended direction
        _projectileParent.transform.localScale = _projectileParent.transform.localScale * _projectileCharge * 1.5f; // adjust scale depending on how much the player charged up the shot

    }

    private void FixedUpdate()
    {
        _projectileSpriteGameObject.transform.position = transform.position;
        _projectileSpriteGameObject.transform.LookAt(transform.forward);
        _projectileSprite.flipX = !(transform.eulerAngles.y > 180f);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) // on collision with another object
    {
        Debug.Log("collided with " + collision.gameObject);
        if (collision.gameObject.CompareTag("Enemy")) // if collided object is an enemy
        {
            _collidedEnemy = collision.gameObject; // store collided object as a var
            _collidedEnemyScript = _collidedEnemy.GetComponent<mothController>(); // get collided enemies controller script
            _collidedEnemyScript.Damage(_projectileDamage); // deal damage to the enemy
            Despawn(); // despawn the bullet
        }
        else if (collision.gameObject.CompareTag("furnace"))
        {
            _collidedFurnace = collision.gameObject;
            _collidedFurnaceScript = _collidedFurnace.GetComponent<furnaceScript>();
            _collidedFurnaceScript.AddProgress(_projectileDamage);
            Despawn();
        }
        else
        {
            Despawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("decoy"))
        {
            _collidedDecoy = other.gameObject;
            _decoyScript = _collidedDecoy.GetComponent<lightDecoyPawn>();
            _decoyScript.ActivateDecoy();
        }
    }

    public void Init(float projectileSpeed, float projectileDamage, float projectileDespawnRate, float projectileCharge) // set the variables once instantiated
    {
        _projectileSpeed = projectileSpeed;
        _projectileDamage = projectileDamage;
        _projectileDespawnRate = projectileDespawnRate;
        _projectileCharge = projectileCharge;
    }

    private IEnumerator DespawnCountdown() // despawn counter function
    {
        yield return new WaitForSeconds(_projectileDespawnRate); // wait length of time that the var is set to
        Despawn(); // despawn the bullet
    }

    private void Despawn() // despawn the bullet
    {
        Destroy(transform.parent.gameObject);
    }
}
