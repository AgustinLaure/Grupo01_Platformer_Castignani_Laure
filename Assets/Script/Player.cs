using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthPoints healthPoints;

    [SerializeField] private AreaTrigger attackAreaTrigger;

    [SerializeField] private float damage;
    [SerializeField] private float initialCurrentHealth;
    [SerializeField] private float initialMaxHealth;

    private bool canMove = true;

    public HealthPoints GetHealthPoints { get { return healthPoints; } }
    
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    private void Awake()
    {
        healthPoints = new HealthPoints(initialCurrentHealth, initialMaxHealth);

        healthPoints.OnDie += HandleDie;
        attackAreaTrigger.OnTrigger += HandleAttackAreaTrigger;
    }

    private void HandleAttackAreaTrigger(Collider2D collider)
    {
        //Hit enemy logic
    }
    private void HandleDie()
    {

    }
    private void OnDestroy()
    {
        healthPoints.OnDie -= HandleDie;
        attackAreaTrigger.OnTrigger -= HandleAttackAreaTrigger;
    }
}
