using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    [SerializeField]
    private GameObject _light;
    [SerializeField]
    private bool _on = false;
    private bool _allowInteract = false;
    private void Update()
    {
        if(_allowInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _on = !_on;
                _light.SetActive(_on);
            }
        }
    }

    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _allowInteract = true;
        }
    }
}
