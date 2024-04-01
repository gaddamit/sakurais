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
 
    public virtual void Throw(Transform spawnLocation)
    {
        gameObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * _projectileSpeed;
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
                AIController aIController = other.gameObject.GetComponentInParent<AIController>();
                aIController?.TakeDamage();
                Despawn();
            }
        }
        else
        {
            if(!other.CompareTag("Player"))
            {
                Despawn();
            }
        }
    }

    public virtual void Despawn()
    {
        CancelInvoke();
        Destroy(this.gameObject);
    }
}
