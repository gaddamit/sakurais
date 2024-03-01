using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 10;
    [SerializeField]
    private float _projectileDeathTime = 3;

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
        Invoke("Despawn", _projectileDeathTime);
    }

    public void Throw(Transform spawnLocation)
    {
        gameObject.GetComponent<Rigidbody>().velocity = spawnLocation.forward * _projectileSpeed;
        Invoke("Despawn", _projectileDeathTime);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.name);
        if(!other.CompareTag("Player"))
        {
            Despawn();
        }
    }

    public virtual void Despawn()
    {
        CancelInvoke();
        Destroy(this.gameObject);
    }
}
