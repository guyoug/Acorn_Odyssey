using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Elite : MonoBehaviour
{
    [Header("Status")]
    public int Hp = 15;
    public int moveSpeed = 3;
    public float burstDelay = 0.12f;   
    public float knifeDelay = 0.7f;   
    private float GroupDelay = 1.2f;
    private bool isDead = false;
  

    [Header("Movement Range")]
    private float minX = 3.5f;
    private float maxX = 7.0f;
    private float minY = -2.0f;
    private float maxY = 3.3f;
    private Vector3 targetPos;

    [Header("Prefabs & Items")]
    public GameObject knifePrefab;
    public GameObject bulletPrefab;
    public GameObject[] dropItems;
   

    [Header("Fire Points")]
    public Transform throwPoint;
    public Transform firePoint;

    [Header("References")]
    private GameManager gameManager;
    private Rigidbody2D rb;

    public void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        SetNewTarget();  // 시작 시 최초 목표 위치 설정
        StartCoroutine(PattenLoop());
    }
    IEnumerator PattenLoop()
    {
        while (true)
        {
            if (isDead)
                yield break;
           
            yield return StartCoroutine(BrustAttack());

            yield return StartCoroutine(KnifeAttack());

            yield return new WaitForSeconds(GroupDelay);
        }
           
    }


    IEnumerator BrustAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(burstDelay);
        }
    }
       
    IEnumerator KnifeAttack()
    {
        Instantiate(knifePrefab, throwPoint.position, throwPoint.rotation);
        yield return new WaitForSeconds(knifeDelay);
    }
    private void FixedUpdate()
    {
        MoveRandom();
    }

    void MoveRandom()
    {
        if (rb == null)
            return;
        // 현재 위치에서 targetPos 방향으로 이동
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        // 목표 위치에 거의 도달하면 새로운 목표 설정
        if (Vector2.Distance(rb.position, targetPos) < 0.1f)
            SetNewTarget();
    }

    void SetNewTarget()    // 새로운 랜덤 이동 목표 설정
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        targetPos = new Vector3(x, y, transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject); 
        }
    }
    public void TakeDamage(int dmg)
    {
        if (isDead)
           return;
        Hp -= dmg;
        Debug.Log($"엘리트 남은 HP : {Hp}");
        if (Hp <= 0)
            Die();
    }
    public void Die()
    {
        if (isDead)
           return;
        isDead = true;
        gameManager.OnEliteEnemyKilled();
        DropItem();
        Destroy(gameObject);
    }
    // 아이템 드롭 처리 (무작위 1개)
    private void DropItem()
    {
        int idx = Random.Range(0, dropItems.Length);
        Instantiate(dropItems[idx], transform.position, Quaternion.identity);
    }
}

