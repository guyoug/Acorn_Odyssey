using System.Collections;
using UnityEngine;

public class Stage2_Elite : MonoBehaviour
{
    [Header("Status")]
    public int Hp = 15;
    private bool isDead = false;
    private bool isPatternRunning = false;

    [Header("Movement Settings")]
    public float maxSpeed = 2f;
    private bool movingUp = true;
    private float minY = -2.0f;
    private float maxY = 3.3f;

    [Header("Shoot Pattern")]
    public GameObject enemyPrefabs;
    public Transform firePoint;

    [Header("Dash Pattern")]
    public float dashSpeed = 8f;
    public float dashDistance = 6f; 
    public float returnSpeed = 4f;
    public float returnDuration = 0.6f;
    public GameObject exclamationUI;

    [Header("Warning UI")] 
    public GameObject warningUIObj;
    private Elite2WarningBlink warningUI;

    [Header("Drop Items")]
    public GameObject[] dropItems;

    [Header("References")]
    private GameManager gameManager;
    private Transform player;

    [Header(" Sprite Effect")]
    public Sprite normalSprite;      // 기본 이미지
    public Sprite shootSprite;       // 공격 시 이미지
    public float shootSpriteTime = 0.15f;
    public Sprite deadSprite;
    private Collider2D col;
    private SpriteRenderer sr;
    private float DeadSprite = 0.3f;

    [Header("Hit Flash")]
    private Coroutine hitFlashRoutine;

    private Vector3 spawnPosition;

    void Awake()
    {
        gameManager = GameManager.Instance;
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (firePoint == null)
            firePoint = transform.Find("FirePoint");

        warningUI = warningUIObj.GetComponent<Elite2WarningBlink>();
 

        if (exclamationUI == null)
            exclamationUI = transform.Find("ExclamationUI")?.gameObject;

        if (exclamationUI != null)
            exclamationUI.SetActive(false);

        spawnPosition = transform.position;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && normalSprite == null)
            normalSprite = sr.sprite;

        StartCoroutine(PatternLoop());
    }

    void FixedUpdate()
    {
        if (!isPatternRunning && !isDead)
            Move();
    }
 
    IEnumerator PatternLoop()
    {
        yield return new WaitForSeconds(3f); 
        while (!isDead)
        {
            spawnPosition = transform.position;
            if (warningUI != null)
                yield return StartCoroutine(warningUI.Blink());

            // 1️ Shoot
            ShootEnemy();

            isPatternRunning = true;
            // 2️ 2초 대기
            yield return new WaitForSeconds(1f);

            // 3 느낌표 표시
            yield return StartCoroutine(ShowExclamation());

            // 4️ Dash
            yield return StartCoroutine(DashAttack());
      
            // 6️ 1초 휴식
            yield return new WaitForSeconds(1f);

            isPatternRunning = false;
        }
    }

    void ShootEnemy()
    {
        StartCoroutine(ShootSpriteEffect());
        Instantiate(enemyPrefabs, firePoint.position, firePoint.rotation);
        Debug.Log("Shoot Enemy");
    }

  
    IEnumerator ShowExclamation()
    {
        if (exclamationUI != null)
        {
            exclamationUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            exclamationUI.SetActive(false);
        }
    }

    IEnumerator DashAttack()
    {
        if (player == null)
            yield break;

        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + dir * dashDistance;

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }
        yield return StartCoroutine(ReturnToSpawn());

    }


    IEnumerator ReturnToSpawn()
    {
        Vector3 startPos = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / returnDuration;

            float easedT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, spawnPosition, easedT);
            yield return null;
        }

        transform.position = spawnPosition;
    }

    IEnumerator ShootSpriteEffect()
    {
        if (sr == null || shootSprite == null)
            yield break;

        sr.sprite = shootSprite;
        yield return new WaitForSeconds(shootSpriteTime);
        sr.sprite = normalSprite;
    }
    IEnumerator HitFlash()
    {
        if (sr == null || isDead) yield break;

        sr.color = new Color(1f, 0.4f, 0.4f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    void Move()
    {
        Vector3 pos = transform.position;

        if (movingUp)
        {
            pos.y += maxSpeed * Time.deltaTime;
            if (pos.y >= maxY)
                movingUp = false;
        }
        else
        {
            pos.y -= maxSpeed * Time.deltaTime;
            if (pos.y <= minY)
                movingUp = true;
        }

        transform.position = pos;
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            hitFlashRoutine = StartCoroutine(HitFlash());
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead)
            return;

        Hp -= dmg;
        Debug.Log($"엘리트 HP: {Hp}");

        if (Hp <= 0)
            Die();
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        StopAllCoroutines();

        gameManager.OnEliteEnemyKilled();
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
            //anim.enabled = false;
            sr.sprite = deadSprite;
        }


        if (col != null)
            col.enabled = false;

         maxSpeed = 0f;


        yield return new WaitForSeconds(DeadSprite);

        Destroy(gameObject);
    }
    void DropItem()
    {
        if (dropItems.Length == 0)
            return;

        int idx = Random.Range(0, dropItems.Length);
        Instantiate(dropItems[idx], transform.position, Quaternion.identity);
    }
}