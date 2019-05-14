using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    private float healt = 10.0f;
    public void TakeDamage(float amount)
    {
        healt -= amount;
        if (healt <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
