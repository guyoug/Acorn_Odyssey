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

    }

}
