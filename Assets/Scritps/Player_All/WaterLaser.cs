using UnityEngine;

public class WaterLaser : MonoBehaviour
{
    [Header("Laser Settings")]
    public int lifeTime = 2;
    private float damageInterval = 1f; 
    private float damageTimer = 0f;

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

        if (collision.CompareTag("EnemyBullet"))
            Destroy(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        damageTimer += Time.deltaTime;
        if (damageTimer < damageInterval)
            return;

        damageTimer = 0f;

        if (collision.CompareTag("Elite"))
        {
            Elite elite = collision.GetComponent<Elite>();
            elite.TakeDamage(5);
        }

        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            boss.TakeDamage(5); 
        }

    }

}
