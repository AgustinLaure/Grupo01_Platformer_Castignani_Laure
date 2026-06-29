using UnityEngine;
using System;

public class MeleeEnemy : Enemy
{
    public event Action OnAttack;

    [SerializeField] private GameObject attackArea;
    [SerializeField] private float attackCooldown;

    [SerializeField] private AreaTrigger[] controlPoints;
    [SerializeField] private float acceleration;
    [SerializeField] private Vector2 terminalVelocity;
    [SerializeField] private ForceMode2D forceMode;

    private int currentControlPointIndex = 0;

    private AreaTrigger attackAreaTrigger;
    private Rigidbody2D rb;

    private bool canAttack = true;

    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        attackAreaTrigger = attackArea.GetComponent<AreaTrigger>();

        foreach (AreaTrigger trigger in controlPoints)
        {
            trigger.OnTrigger += HandleControlPointCollision;
        }

        attackAreaTrigger.OnTrigger += HandleAttackAreaCollision;
    }


    private void Update()
    {
        Attack();
        Patrol();
    }

    private void Patrol()
    {
        Vector2 targetDir = (controlPoints[currentControlPointIndex].transform.position - transform.position).normalized;

        rb.AddForce(new Vector2(targetDir.x * acceleration, 0f), forceMode);

        if (rb.linearVelocityX > terminalVelocity.x)
        {
            rb.linearVelocityX = terminalVelocity.x;
        }
    }

    private void ChangeControlPoint()
    {
        if (currentControlPointIndex + 1 < controlPoints.Length)
        {
            currentControlPointIndex++;
        }
        else
        {
            currentControlPointIndex = 0;
        }
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

        foreach (AreaTrigger trigger in controlPoints)
        {
            trigger.OnTrigger -= HandleControlPointCollision;
        }
    }

    private void HandleAttackAreaCollision(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().GetHealthPoints.TakeDamage(damage);
        }
    }

    private void HandleControlPointCollision(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            ChangeControlPoint();
        }
    }
}
