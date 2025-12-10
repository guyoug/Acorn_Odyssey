using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public int maxSpeed = 1;
    private Vector2 shootDirection;

    [Header("Lifetime Settings")]
    private float deleteTime = 10.0f;

    [Header("References")]
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Transform player = GameObject.FindWithTag("Player").transform;
        if (player != null)
            shootDirection = (player.position - transform.position).normalized;
        Destroy(gameObject, deleteTime);
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
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outline"))
            Destroy(gameObject);
    }
}