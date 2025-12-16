using UnityEngine;

public class Stage1_Elite_Bullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public int Damage = 1; 
    public float maxSpeed = 10.0f;
    private float lifeTime = 10.0f;

    private Vector2 shootDirection;

    [Header("References")]
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //플레이어 위치로 발사 방향 계산
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
        Destroy(gameObject,lifeTime);
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
