using UnityEngine;
public abstract class Enemy : MonoBehaviour
{
    private HealthPoints healthPoints;

    [SerializeField] protected float damage;

    public HealthPoints GetHealthPoints { get { return healthPoints; } }
}
