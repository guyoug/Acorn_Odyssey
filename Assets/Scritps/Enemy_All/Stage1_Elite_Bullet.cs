using UnityEngine;

public class Stage1_Elite_Bullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public int Damage = 1; 
    public float maxSpeed = 10.0f;
    private Vector2 shootDirection;

    [Header("Lifetime Settings")]
    private float deleteTime = 10.0f;

    [Header("References")]
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            shootDirection = (playerObj.transform.position - transform.position).normalized;
        }
        else
        {
            // 플레이어 없으면 그냥 왼쪽으로 발사하거나 즉시 제거
            shootDirection = Vector2.left;
        }
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
            PlayerHealth.Instance.TakeDamage(Damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }
    }
}
