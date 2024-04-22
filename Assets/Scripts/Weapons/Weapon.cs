using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject _owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Player and enemy has different collider types
    // Handles stabbing damage
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(other.GetType() == typeof(CapsuleCollider))
            {
                if(other.isTrigger)
                {
                    AIController aiController = other.gameObject.GetComponent<AIController>();
                    Damage damage = gameObject.GetComponent<Damage>();
                    aiController.ApplyDamage(_owner, damage.DamageAmount);
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
