using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float health; 
    public float maxHealth;
    public Image HealthBar; // reference to our players health bar. 
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health; // Players max health will be the players health at the start of the game.
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1); // health bar wont be able to exceed the players max health.

        if (health <= 0) 
        {
            Destroy(gameObject); // If the players health goes below or equal to 0, then the player will be destroyed. 
        }
    }
}
