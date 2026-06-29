using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthPoints healthPoints;

    [SerializeField] private AreaTrigger attackAreaTrigger;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dieSound;

    [SerializeField] private float damage;
    [SerializeField] private float initialCurrentHealth;
    [SerializeField] private float initialMaxHealth;

    private PlayerController controller;

    private bool canMove = true;

    public HealthPoints GetHealthPoints { get { return healthPoints; } }
    
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    private void Awake()
    {
        healthPoints = new HealthPoints(initialCurrentHealth, initialMaxHealth);

        controller = GetComponent<PlayerController>();

        controller.OnPlayerJump += HandleJump;
        healthPoints.OnDie += HandleDie;
        attackAreaTrigger.OnTrigger += HandleAttackAreaTrigger;
    }

    private void HandleAttackAreaTrigger(Collider2D collider)
    {
        //Hit enemy logic
    }

    private void HandleDie()
    {
        dieSound.Play();
    }
    private void HandleJump()
    {
        jumpSound.Play();
    }

    private void OnDestroy()
    {
        controller.OnPlayerJump += HandleJump;
        healthPoints.OnDie -= HandleDie;
        attackAreaTrigger.OnTrigger -= HandleAttackAreaTrigger;
    }
}
