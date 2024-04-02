using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _bulbLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Turn off the light when the projectile hits the lamp
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if(_light != null)
            {
                _light.SetActive(false);
            }
            
            if(_bulbLight != null)
            {
                _bulbLight.SetActive(false);
            }
        }
    }
}
