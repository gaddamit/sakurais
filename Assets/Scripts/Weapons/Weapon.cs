using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(other.GetType() == typeof(CapsuleCollider))
            {
                if(other.isTrigger)
                {
                    PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
                    Damage damage = gameObject.GetComponent<Damage>();
                    playerHealth.ApplyDamage(damage.DamageAmount);
                }
            }
        }
        
        if(other.gameObject.tag == "Player")
        {
            if(other.GetType() == typeof(CapsuleCollider))
            {
                if(!other.isTrigger)
                {
                    PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
                    Damage damage = gameObject.GetComponent<Damage>();
                    playerHealth.ApplyDamage(damage.DamageAmount);
                }
            }
        }
    }
}
