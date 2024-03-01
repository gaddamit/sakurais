using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NinjaStar : Projectile
{
    private void Start()
    {

    }

    private void Update()
    {

    }

    public override void Spawn(Transform spawnLocation)
    {
        base.Spawn(spawnLocation);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void Despawn()
    {
        Debug.Log("NinjaStar despawned");
        base.Despawn();
    }
}
