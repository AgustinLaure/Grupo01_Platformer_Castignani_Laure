using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthPoints healthPoints;

    [SerializeField] private float initialCurrentHealth;
    [SerializeField] private float initialMaxHealth;

    public HealthPoints GetHealthPoints { get { return healthPoints; } }

    private void Awake()
    {
        healthPoints = new HealthPoints(initialCurrentHealth, initialMaxHealth);
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

    private void HandleTakeDamage()
    {

    }

    private void HandleDie()
    {

    }
}
