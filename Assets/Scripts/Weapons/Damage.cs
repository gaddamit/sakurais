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

    // Apply damage to gameObject's playerHealth script
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected");
        if (other.gameObject.CompareTag("Player"))
        {
            pHealth.ApplyDamage(_damage);
        }
    }
}
