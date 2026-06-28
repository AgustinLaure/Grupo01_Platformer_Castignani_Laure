using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthPoints healthPoints;

    [SerializeField] private float initialCurrentHealth;
    [SerializeField] private float initialMaxHealth;

    private bool isDead = false;

    public HealthPoints GetHealthPoints { get { return healthPoints; } }
    public bool IsDead { get { return isDead; } }

    private void Awake()
    {
        healthPoints = new HealthPoints(initialCurrentHealth, initialMaxHealth);

        healthPoints.OnDie += HandleDie;
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

    private void OnDestroy()
    {
        healthPoints.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {

    }

    private void HandleDie()
    {
        isDead = true;
    }
}
