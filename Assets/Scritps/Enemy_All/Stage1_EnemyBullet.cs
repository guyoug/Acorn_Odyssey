using UnityEngine;

public class Stage1_EnemyBullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public int maxSpeed = 1;
    [Header("Damage Settings")]
    public int Damage = 1;

    [Header("Lifetime Settings")]
    private float lifeTime = 10.0f;

    [Header("References")]
    private Rigidbody2D rb;

    private Vector2 shootDirection;


    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Transform player = GameObject.FindWithTag("Player").transform;
        if (player != null)
            shootDirection = (player.position - transform.position).normalized;
        Destroy(gameObject, lifeTime);
    }
    void FixedUpdate()
    {
        MoveBullet();
    }
    private void MoveBullet()
    {
        rb.linearVelocity = shootDirection * maxSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(Damage);
            
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outline"))
            Destroy(gameObject);
    }
}