using System;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public event Action OnTrigger;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTrigger?.Invoke();
    }
}
