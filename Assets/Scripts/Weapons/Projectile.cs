using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.SearchService;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 10;
    [SerializeField]
    private float _projectileDeathTime = 3;

    private CinemachineImpulseSource _impulseSource;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
 
    // Throw the projectile
    public virtual void Throw(Transform spawnLocation)
    {
        gameObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * _projectileSpeed;

        // Add impulse to the camera as an effect
        _impulseSource = gameObject.GetComponent<CinemachineImpulseSource>();
        _impulseSource.GenerateImpulse(Camera.main.transform.forward);

        Invoke("Despawn", _projectileDeathTime);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(other.GetType() == typeof(CapsuleCollider))
            {
                //Apply damage to the receiving object
                PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
                Damage damage = gameObject.GetComponent<Damage>();
                playerHealth.ApplyDamage(damage.DamageAmount);

                Despawn();
            }
        }
        else
        {
            // Despawn the projectile if it hits anything other than the player or a weapon
            if(!other.CompareTag("Player") && !other.CompareTag("Weapon"))
            {
                Despawn();
            }
        }
    }

    // Despawn the projectile
    public virtual void Despawn()
    {
        CancelInvoke();
        Destroy(this.gameObject);
    }
}
