using System.Collections;
using UnityEngine;

public class Stage1_Elite : MonoBehaviour
{
    [Header("Status")]
    public int Hp = 15;
    public int moveSpeed = 3;
    public float burstDelay = 0.12f;   
    public float knifeDelay = 0.7f;   
    private float patternDelay = 1.2f;
  
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

    [Header("Death Sprite")]
    public Sprite deadSprite;
    private float deadSpriteTime = 0.3f;

    [Header("Hit Flash")]
    public float hitFlashTime = 0.1f;

    [Header("References")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    private Coroutine hitFlashRoutine;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        SetNewTarget();  
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

            yield return new WaitForSeconds(patternDelay);
        }
           
    }
    IEnumerator BrustAttack() // 3발 점사
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(burstDelay);
        }
    }
       
    IEnumerator KnifeAttack() // 칼 투척
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
        //tragetpos로 이동
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
        hitFlashRoutine = StartCoroutine(HitFlash());
        Debug.Log($"엘리트 남은 HP : {Hp}");
        if (Hp <= 0)
            Die();
    }
    IEnumerator HitFlash()
    {
        if (sr == null || isDead) yield break;

        sr.color = new Color(1f, 0.4f, 0.4f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    public void Die()
    {
        if (isDead)
           return;
        isDead = true;
        PlayerHealth.Instance.StartCoroutine(PlayerHealth.Instance.SetInvincible(2f));
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);
        GameManager.Instance.OnEliteEnemyKilled();
        DropItem();
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        StartCoroutine(DieRoutine());
       
    }
    IEnumerator DieRoutine()
    {
        if (sr != null && deadSprite != null)
        {
            sr.color = Color.white;
            anim.enabled = false;
            sr.sprite = deadSprite;
        }

        if (col != null)
            col.enabled = false;

        moveSpeed = 0;

        yield return new WaitForSeconds(deadSpriteTime);

        Destroy(gameObject);
    }
    // 아이템 드롭 처리 (무작위 1개)
    private void DropItem()
    {
        int idx = Random.Range(0, dropItems.Length);
        Instantiate(dropItems[idx], transform.position, Quaternion.identity);
    }
   
  
}

