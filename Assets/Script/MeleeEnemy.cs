using UnityEngine;
using System;

public class MeleeEnemy : Enemy
{
    public event Action OnAttack;

    [SerializeField] private GameObject attackArea;
    [SerializeField] private float attackCooldown;

    private AreaTrigger attackAreaTrigger;

    private bool canAttack = true;

    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    private void Awake()
    {
        attackAreaTrigger.OnTrigger += HandleAttackAreaCollision;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        attackArea.SetActive(true);
        canAttack = false;
        OnAttack?.Invoke();
    }

    private void OnDestroy()
    {
        attackAreaTrigger.OnTrigger -= HandleAttackAreaCollision;
    }

    private void HandleAttackAreaCollision(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().GetHealthPoints.TakeDamage(damage);
        }
    }
}
