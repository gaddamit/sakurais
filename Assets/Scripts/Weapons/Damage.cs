using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public PlayerHealth pHealth;
    [SerializeField]
    private float _damage;
    public float DamageAmount
    {
        get { return _damage; }
        set { _damage = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))// If the player hits the object he will take damage. Player's tag HAS to be "Player"
        {
            pHealth.ApplyDamage(_damage);// Player will take damage to whatever value we want to set it as. 
        }
    }
}
