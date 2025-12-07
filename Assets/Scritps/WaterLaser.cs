using UnityEngine;

public class WaterLaser : MonoBehaviour
{
    [Header("Lifetime Settings")]
    public int lifeTime = 2;

    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Die();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Elite"))
        {
            Elite elite = collision.GetComponent<Elite>();
            elite.TakeDamage(10);
        }

        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            boss.TakeDamage(10);
        }

        if (collision.CompareTag("EnemyBullet"))
            Destroy(collision.gameObject);
    }

}
