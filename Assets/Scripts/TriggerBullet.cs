using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBullet : MonoBehaviour
{
    private int health = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            health -= 1;
            Die(); // Добавляем вызов метода Die() после уменьшения значения здоровья
        }
    }

    private void Die()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}