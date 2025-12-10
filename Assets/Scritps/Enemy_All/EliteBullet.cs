using UnityEngine;

public class EliteBullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10.0f;
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
            shootDirection = (player.position - transform.position).normalized; // 방향 벡터 정규화 (거리와 상관없이 방향만 유지)
        Destroy(gameObject, deleteTime);
    }
    void FixedUpdate()
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
        {
            Destroy(gameObject);
        }
    }
}
