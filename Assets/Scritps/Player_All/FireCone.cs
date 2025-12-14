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
            Stage1_Enemy stage1_enemy = collision.GetComponent<Stage1_Enemy>();
            if (stage1_enemy != null)
            {
                stage1_enemy.Die();

            }
            Stage2_Enemy stage2_Enemy = collision.GetComponent<Stage2_Enemy>();
            if (stage2_Enemy != null)
            {
                stage2_Enemy.Die();
            }
            Stage3_Enemy stage3_Enemy = collision.GetComponent<Stage3_Enemy>();
            if (stage3_Enemy != null)
            {
                stage3_Enemy.Die();
            }
        }
    
        if (collision.CompareTag("Elite"))
        {
            Stage1_Elite stage1_Elite = collision.GetComponent<Stage1_Elite>();
            if (stage1_Elite != null)
                stage1_Elite.TakeDamage(5);

        }

        Stage2_Elite stage2_Elite = collision.GetComponent<Stage2_Elite>();
        if (stage2_Elite != null)
            stage2_Elite.TakeDamage(5);


        //Stage3_Elite stage3_Elite = collision.GetComponent<Stage3_Elite>();
        //if(stage3_Elite != null)
        // stage3_Elite.TakeDamage(5);
        //   
        //  
        //}


        if (collision.CompareTag("Boss"))
        {
            Stage1_Boss stage1_Boss = collision.GetComponent<Stage1_Boss>();
            if (stage1_Boss != null)
            {
                stage1_Boss.TakeDamage(5);

            }
            Stage2_Boss stage2_Boss = collision.GetComponent<Stage2_Boss>();
            if (stage2_Boss != null)
            {
                stage2_Boss.TakeDamage(5);
            }
            Stage3_Boss stage3_Boss = collision.GetComponent<Stage3_Boss>();
            if (stage3_Boss != null)
            {
                stage3_Boss.TakeDamage(5);

            }
        }

        if (collision.CompareTag("EnemyBullet"))
            Destroy(collision.gameObject);
    }
}