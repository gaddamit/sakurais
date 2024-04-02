using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    private bool _isDead = false;
    public bool IsDead
    {
        get { return _isDead; }
    }
    [SerializeField]
    private float _health;
    [SerializeField] 
    private float _maxHealth;
    [SerializeField]
    private Image _healthBar; // reference to our players health bar. 

    public delegate void OnPlayerDeath();
    public OnPlayerDeath onPlayerDeathDelegate;

    // Start is called before the first frame update
    void Start()
    {
        _maxHealth = _health; // Players max health will be the players health at the start of the game.
    }

    // Update is called once per frame
    void Update()
    {
        if(_healthBar != null)
        {
            _healthBar.fillAmount = Mathf.Clamp(_health / _maxHealth, 0, 1); // health bar wont be able to exceed the players max health.
        }
    }

    public void ApplyDamage(float damage)
    {
        _health -= damage; // Players health will decrease by the amount of damage that is applied to the player. 
        CheckForDeath(); // Check if the player has died.
    }

    private void CheckForDeath()
    {
        if (!_isDead && _health <= 0)
        {
            _isDead = true;
            onPlayerDeathDelegate?.Invoke(); // If the players health goes below or equal to 0, then the player will die.
        }
    }
}
