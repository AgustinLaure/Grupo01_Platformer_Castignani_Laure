using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().GetHealthPoints.TakeDamage(damage);
        }
    }
}
