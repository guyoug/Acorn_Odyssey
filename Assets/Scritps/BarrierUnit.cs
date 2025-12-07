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
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
                hitBarrier();
            }
        }
    }
    void hitBarrier()
    {
        hp--;
        Debug.Log($"밤송이 피격! 남은 HP = {hp}"); ;
        if (hp <= 0)
        {
            manager.RemoveBarrier(this);
            Destroy(gameObject);
        }
    }
}

