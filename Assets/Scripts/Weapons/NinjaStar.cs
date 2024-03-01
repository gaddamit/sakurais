using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NinjaStar : Projectile
{
    [SerializeField]
    private float _rotationSpeed = 5.0f;
    [SerializeField]
    private Vector3 _rotationDirection = new Vector3(0, 0, 0);
    private void Start()
    {

    }

    private void Update()
    {
        transform.Rotate(_rotationDirection, 100 * _rotationSpeed * Time.deltaTime);
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
        base.Despawn();
    }
}
