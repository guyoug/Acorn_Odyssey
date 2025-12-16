using Unity.VisualScripting;
using UnityEngine;

public class FireCone : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float Hp = 3;
    public float maxSpeed = 10f;
    public float lifeTime = 4f;

    [Header("Runtime")]
    private Rigidbody2D rb;

    public Transform target;

    void Start()
    {
    
        Destroy(gameObject, lifeTime);

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * maxSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") || collision.CompareTag("Elite") || collision.CompareTag("Boss"))
        {
           Destroy(gameObject);
        }

        Stage1_Enemy stage1_Enemy = collision.GetComponent<Stage1_Enemy>();
        if (stage1_Enemy != null)
        {
            stage1_Enemy.TakeDamage(1);
        }
        Stage2_Enemy stage2_Enemy = collision.GetComponent<Stage2_Enemy>();
        if (stage2_Enemy != null)
        {
            stage2_Enemy.TakeDamage(1);
        }
        Stage3_Enemy stage3_Enemy = collision.GetComponent<Stage3_Enemy>();
        if (stage3_Enemy != null)
        {
            stage3_Enemy.TakeDamage(1);
        }

        Stage1_Elite stage1_Elite = collision.GetComponent<Stage1_Elite>();
        if (stage1_Elite != null)
            stage1_Elite.TakeDamage(1);

        Stage2_Elite stage2_Elite = collision.GetComponent<Stage2_Elite>();
        if (stage2_Elite != null)
            stage2_Elite.TakeDamage(1);

        Stage3_Elite stage3_Elite = collision.GetComponent<Stage3_Elite>();
        if (stage3_Elite != null)

            stage3_Elite.TakeDamage(1);


        Stage1_Boss stage1_Boss = collision.GetComponent<Stage1_Boss>();
        if (stage1_Boss != null)
        {
            stage1_Boss.TakeDamage(1);

        }
        Stage2_Boss stage2_Boss = collision.GetComponent<Stage2_Boss>();
        if (stage2_Boss != null)
        {
            stage2_Boss.TakeDamage(1);
        }
        Stage3_Boss stage3_Boss = collision.GetComponent<Stage3_Boss>();
        if (stage3_Boss != null)
        {
            stage3_Boss.TakeDamage(1);
        }
    }
}

