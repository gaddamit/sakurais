using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public PlayerHealth pHealth;
    public float damage;
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
            pHealth.health -= damage;// Player will take damage to whatever value we want to set it as. 
        }
    }
}
