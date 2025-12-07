using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Elite : MonoBehaviour
{
    [Header("Status")]
    public int Hp = 15;
    public int moveSpeed = 3;
    public float attackDelay = 2.5f;
    public float movelength = 1.5f; 
    public float burstDelay = 0.12f; 
    private bool isDead = false;
    private float attackTimer = 0f;
    public float knifeInterval = 5f;  
    private float knifeTimer = 0f;

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
    }
    public void Update()
    {
        BristttackUpdate();
        KnifeAttackUpdate();
    }
    void BristttackUpdate()   //  일정 시간마다 공격
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0f;
            StartCoroutine(brustShoot());
        }
    }
    void KnifeAttackUpdate()
    {
        knifeTimer += Time.deltaTime;

        if (knifeTimer >= knifeInterval)
        {
            knifeTimer = 0f;
            ThrowKnife();   
        }
    }
    private void FixedUpdate()
    {
        MoveRandom();
    }
    void ThrowKnife()
    {
        Instantiate(knifePrefab, throwPoint.position, throwPoint.rotation);
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

    // 3연사 공격 코루틴
    IEnumerator brustShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(burstDelay);
        }
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

