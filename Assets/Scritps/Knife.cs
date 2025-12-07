using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Knife Settings")]
    public float speed = 25f;
    public float curveStrength = 2f;
    public float upwardForce = 5.5f;     
    public float gravity = 2.2f;       
    public float lifeTime = 5f;
    public int damage = 1;
    public float angleOffset = 20f;

    [Header("Runtime")]
    private Rigidbody2D rb;
    private Vector2 horizontalDir;
    private float verticalVelocity;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Transform player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            horizontalDir = new Vector2(dir.x, 0).normalized;

            verticalVelocity = upwardForce * 0.3f;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle+ 180f);
        }

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        verticalVelocity -= gravity * Time.deltaTime;

        Vector3 move =
            (Vector3)horizontalDir * speed * Time.deltaTime
            + new Vector3(0, verticalVelocity * Time.deltaTime, 0);

        transform.position += move;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth hp = collision.GetComponent<PlayerHealth>();
            hp.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }



    }
    
}
