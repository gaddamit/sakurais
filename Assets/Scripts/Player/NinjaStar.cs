using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    public Transform ninjaStarSpawnPoint;
    public GameObject ninjaStarPrefab;
    public float ninjaStarSpeed = 10;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ninjaStar = Instantiate(ninjaStarPrefab, ninjaStarSpawnPoint.position, ninjaStarSpawnPoint.rotation); 
            ninjaStar.GetComponent<Rigidbody>().velocity = ninjaStarSpawnPoint.forward * ninjaStarSpeed;
        }
    }
}
