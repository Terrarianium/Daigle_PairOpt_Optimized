using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Range(0, 200)] public int startingHealth = 100, currentHealth;

    public bool isDead;

    public event Action OnDeath;

    void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took damage. HP: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log(gameObject.name + " died");

            OnDeath?.Invoke();
        }
    }

    public void ResetHealth()
    {
        isDead = false;
        currentHealth = startingHealth;
    }
}