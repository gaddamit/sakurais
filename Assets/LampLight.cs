using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{
    [SerializeField]
    private GameObject _light;
    [SerializeField]
    private GameObject _bulbLight;
    [SerializeField]
    private Lamp _lamp;

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
        Debug.Log("OnTriggerEnter LampLight");
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
            //_isLightOn = false;
            _lamp.LightOn = false;
            _lamp.ResetAIControllers();
        }
    }
}
