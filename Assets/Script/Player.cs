using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthPoints healthPoints;

    [SerializeField] private AreaTrigger attackAreaTrigger;

    [SerializeField] private float damage;
    [SerializeField] private float initialCurrentHealth;
    [SerializeField] private float initialMaxHealth;

    private bool isDead = false;

    public HealthPoints GetHealthPoints { get { return healthPoints; } }
    public bool IsDead { get { return isDead; } }

    private void Awake()
    {
        healthPoints = new HealthPoints(initialCurrentHealth, initialMaxHealth);

        healthPoints.OnDie += HandleDie;
        attackAreaTrigger.OnTrigger += HandleAttackAreaTrigger;
    }
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            healthPoints.TakeDamage(5f);
        }

        Debug.Log("Health: " + healthPoints.GetCurrentHealth + " / " + healthPoints.GetMaxHealth);
    }

    private void HandleAttackAreaTrigger(Collider2D collider)
    {
        //Hit enemy logic
    }
    private void HandleDie()
    {
        isDead = true;
    }
    private void OnDestroy()
    {
        healthPoints.OnDie -= HandleDie;
        attackAreaTrigger.OnTrigger -= HandleAttackAreaTrigger;
    }
}
