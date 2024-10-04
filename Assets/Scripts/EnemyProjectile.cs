using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int projectileDamage = 20;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(projectileDamage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 1.5f);
        }
    }
}
