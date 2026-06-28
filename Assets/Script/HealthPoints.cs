using System;
using UnityEngine;

public class HealthPoints
{
    public event Action OnDie;
    public event Action OnTakeDamage;

    private float currentHealth;
    private float maxHealth;

    public HealthPoints(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        OnTakeDamage?.Invoke();

        if (currentHealth <= 0f)
        {
            OnDie?.Invoke();
        }
    }
}
