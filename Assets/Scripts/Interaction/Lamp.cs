using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Lamp : MonoBehaviour
{
    [SerializeField]
    private Light _light;
    [SerializeField]
    private Light _bulbLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Lamp OnTriggerEnter");
        if (other.CompareTag("Projectile"))
        {
            if(_light != null)
            {
                _light.enabled = false;
            }

            if(_bulbLight != null)
            {
                _bulbLight.enabled = false;
            }
        }
    }
}
