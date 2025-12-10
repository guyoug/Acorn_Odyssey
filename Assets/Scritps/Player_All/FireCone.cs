using UnityEngine;

public class FireCone : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float angleRange = 30f;
    public float speed = 3f;
    public float lifeTime = 2f;
    private float angle;
    [Header("Runtime")]
    private float baseAngle;
    public Transform target;

    void Start()
    {
      
        baseAngle = transform.eulerAngles.z;
        Destroy(gameObject, lifeTime);
     
    }
    void Update()
    {
       FireConeDamage();    
    }
    private void FireConeDamage()
    {
        Vector3 pos = target.position;
        pos.z = 0;
        pos.x += 0.6f;
        transform.position = pos;
        angle = Mathf.Sin(Time.time * speed) * angleRange;
        transform.rotation = Quaternion.Euler(0, 0, baseAngle + angle);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Die();
        }

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

        if (collision.CompareTag("EnemyBullet"))
            Destroy(collision.gameObject);
    }
}