using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 10;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public virtual void Spawn(Transform spawnLocation)
    {
        GameObject projectile = Instantiate(this.gameObject, spawnLocation.position, this.gameObject.transform.rotation); 
        projectile.GetComponent<Rigidbody>().velocity = spawnLocation.forward * _projectileSpeed;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            Despawn();
        }
    }

    public virtual void Despawn()
    {
        Destroy(this.gameObject);
    }
}
