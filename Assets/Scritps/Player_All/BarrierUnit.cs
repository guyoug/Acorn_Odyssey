using System.Threading;
using UnityEngine;

public class BarrierUnit : MonoBehaviour
{
    [Header("Barrier Unit Status")]
    private int hp = 12;

    [Header("Rotation Settings")]
    public float radius = 3.0f; 

    [Header("References")]
    private Transform player;
    private BarrierManager manager;
    void Start()
    {
        player = transform.parent;
        manager = transform.parent.GetComponent<BarrierManager>();
    }

    public void UpdatePosition(float angle)  // BarrierManager가 매 프레임 호출 → 전달된 angle에 따라 위치를 계산해서 이동
    {
        float rad = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        transform.position = player.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;     // 플레이어 기준 원형 궤도에서 위치 계산
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
          
        
            hitBarrier();

        }
        if (collision.CompareTag("Enemy"))
        {
            Stage1_Enemy stage1_enemy = collision.GetComponent<Stage1_Enemy>();
            if (stage1_enemy != null)
            {
                stage1_enemy.TakeDamage(1);
                hitBarrier();
            }
            Stage2_Enemy stage2_Enemy = collision.GetComponent<Stage2_Enemy>();
            if(stage2_Enemy != null)
            {
               stage2_Enemy.TakeDamage(1);
                hitBarrier();
            }
            Stage3_Enemy stage3_Enemy = collision.GetComponent<Stage3_Enemy>();
            if(stage3_Enemy != null)
            {
                stage3_Enemy.TakeDamage(1);
            }
              
            
        }
        if(collision.CompareTag("Elite"))
        {
            Stage1_Elite stage1_Elite = collision.GetComponent<Stage1_Elite>();
            if (stage1_Elite != null)
            {
                stage1_Elite.TakeDamage(1);
                hitBarrier();
            }

            Stage2_Elite stage2_Elite = collision.GetComponent<Stage2_Elite>();
            if (stage2_Elite != null)
            {
                stage2_Elite.TakeDamage(1);
                hitBarrier();
            }

            Stage3_Elite stage3_Elite = collision.GetComponent<Stage3_Elite>();
            if (stage3_Elite != null)
            {
                stage3_Elite.TakeDamage(1);
                hitBarrier();
            }

        }
        if (collision.CompareTag("Boss"))
        {
            Stage1_Boss stage1_Boss = collision.GetComponent<Stage1_Boss>();
            if (stage1_Boss != null)
            {
                stage1_Boss.TakeDamage(1);
                hitBarrier();
            }
            Stage2_Boss stage2_Boss = collision.GetComponent<Stage2_Boss>();
            if(stage2_Boss != null)
            {
                stage2_Boss.TakeDamage(1);
                hitBarrier();
            }
            Stage3_Boss stage3_Boss = collision.GetComponent<Stage3_Boss>();
            if(stage3_Boss != null)
            {
                stage3_Boss.TakeDamage(1);
                hitBarrier();
            }
        }
    }
    void hitBarrier()
    {
        hp--;
        SoundManager.Instance?.PlaySFX(SoundManager.Instance.BarrierHitSFX);
        Debug.Log($"밤송이 피격! 남은 HP = {hp}"); ;
        if (hp <= 0)
        {
            manager.RemoveBarrier(this);
            Destroy(gameObject);
        }
    }
}

