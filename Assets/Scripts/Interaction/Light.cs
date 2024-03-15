using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{

    public GameObject light;
    public bool on = false;

    void OnTriggerStay(Collider Player)
    {
        if (Player.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !on)
        {
            light.SetActive(true);
            on = true; 
        }
        else if (Player.tag == "Player" && Input.GetKeyDown(KeyCode.E) && on)
        {
            light.SetActive(false);
            on = false;
        }
    }
}
