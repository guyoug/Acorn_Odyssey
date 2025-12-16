using System.Collections;
using UnityEngine;

public class Stage2_Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 120;
    private bool isDead = false;
    private bool canMove = true;
    public int Damage = 1;

    [Header("References")]
    private Transform player;
    private Vector3 spawnPosition;    // 대시 후 복귀 위치
    private SpriteRenderer sr;
    private Collider2D col;
    
    [Header("Boss Shoot Points")]
    public Transform firePointCenter;
    public Transform firePointUp;
    public Transform firePointDown;

    [Header("Warning UI")]
    public WarningBlinkUI centerWarning;
    public WarningBlinkUI upWarning;
    public WarningBlinkUI downWarning;

    [Header("Dash Warning UI")]
    public GameObject exclamationUI;


    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public float shootInterval = 0.3f;
    public float patternDelay = 1f;
    public float PatternInterval = 2f;

    [Header("Shoot Angles")]
    public float centerAngle = 0f;
    public float topAngle = 24f;
    public float bottomAngle = -30f;

    [Header("Dash Pattern")]
    public float dashSpeed = 12f;
    public float dashDuration = 1.0f;
    public float dashCooldown = 4f;
    private bool isDashing = false;



    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    private float minY = -2.0f;
    private float maxY = 3.3f;
    private bool movingUp = true;

    [Header("Shoot Sprite Effect")]
    public Sprite normalSprite;      // 기본 이미지
    public Sprite shootSprite;       // 공격 시 이미지
    public float shootSpriteTime = 0.15f;
  

    [Header("Death Sprite")]
    public Sprite deadSprite;
    private float DeadSprite = 0.3f;

    [Header("Hit Flash")]
    private Coroutine hitFlashRoutine;


    void Start()
    {
      
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        spawnPosition = transform.position;

        if (exclamationUI != null)
            exclamationUI.SetActive(false);
    
        if (sr != null && normalSprite == null)
            normalSprite = sr.sprite;

        StartCoroutine(BossShootPattern());
    }

    void FixedUpdate()
    {
        if (!isDead && canMove)
            Move();
    }

    IEnumerator BossShootPattern()
    {
        yield return new WaitForSeconds(PatternInterval); // 나오면 2초 기다리고

        while (!isDead)
        {
            //가운데 부제 다섯번 소환 
            StartCoroutine(ShootSpriteEffect());

            if (centerWarning != null)
                yield return StartCoroutine(centerWarning.Blink());

            yield return StartCoroutine(
            ShootBurst(firePointCenter, 5, centerAngle));

            yield return new WaitForSeconds(patternDelay);

            //아래 부제 세 번 소환
            StartCoroutine(ShootSpriteEffect());

            if (downWarning != null)
                yield return StartCoroutine(downWarning.Blink());

            yield return StartCoroutine(ShootBurst(firePointDown, 3, bottomAngle));

            yield return new WaitForSeconds(patternDelay);
            //위 부제 세 번 소환
            StartCoroutine(ShootSpriteEffect());

            if (upWarning != null)
                yield return StartCoroutine(upWarning.Blink());

            yield return StartCoroutine(
              ShootBurst(firePointUp, 3, topAngle));

            yield return new WaitForSeconds(patternDelay);

            //박치기
            yield return StartCoroutine(DashAttackBurst(3));


            yield return new WaitForSeconds(dashCooldown);
        


        }
    }


    IEnumerator ShootBurst(Transform firePoint, int count, float angle)
    {
        if (firePoint == null)
            yield break;

        Quaternion rot = Quaternion.Euler(0, 0, angle);


        for (int i = 0; i < count; i++)
        {
            Instantiate(bulletPrefab, firePoint.position, rot);
            yield return new WaitForSeconds(shootInterval);
        }
    }
    IEnumerator DashAttackBurst(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (isDead)
                yield break;

            yield return StartCoroutine(DashAttack());
            yield return new WaitForSeconds(0.5f); // 박치기 사이 텀
        }
    }
    IEnumerator DashAttack()
    {
        isDashing = true;
        canMove = false;
        if (player == null)
            yield break;
        spawnPosition = transform.position;

        if (exclamationUI != null)
            exclamationUI.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (exclamationUI != null)
            exclamationUI.SetActive(false);

        Vector3 dir = (player.position - transform.position).normalized;
        float timer = 0f;

            while (timer < dashDuration)
            {
                transform.position += dir * dashSpeed * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(ReturnToSpawn());

        isDashing = false;
        canMove = true;


    }

    IEnumerator ReturnToSpawn()
    {
        float t = 0f;
        Vector3 startPos = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.6f; // 복귀 시간
            float eased = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(startPos, spawnPosition, eased);
            yield return null;
        }

        transform.position = spawnPosition;
    }
    IEnumerator ShootSpriteEffect()
    {
        if (sr == null || shootSprite == null)
            yield break;

        sr.sprite = shootSprite;
        yield return new WaitForSeconds(1f);
        sr.sprite = normalSprite;
    }
    void Move()
    {
        Vector3 pos = transform.position;

        if (movingUp)
        {
            pos.y += moveSpeed * Time.deltaTime;
            if (pos.y >= maxY)
                movingUp = false;
        }
        else
        {
            pos.y -= moveSpeed * Time.deltaTime;
            if (pos.y <= minY)
                movingUp = true;
        }

        transform.position = pos;
    }
    IEnumerator HitFlash()
    {
        if (sr == null || isDead) yield break;

        sr.color = new Color(1f, 0.4f, 0.4f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    IEnumerator DieRoutine()
    {
        if (sr != null && deadSprite != null)
        {
            sr.color = Color.white;
            sr.sprite = deadSprite;
        }


        if (col != null)
            col.enabled = false;

        moveSpeed = 0f;


        yield return new WaitForSeconds(DeadSprite);

        StopAllCoroutines();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);

            Destroy(collision.gameObject);
        }
        if(collision.CompareTag("Player") && isDashing)
        {
            PlayerHealth.Instance.TakeDamage(Damage);
        }
    }
    public void TakeDamage(int dmg)
    {
        if (isDead)
            return;
        Hp -= dmg;
        hitFlashRoutine = StartCoroutine(HitFlash());
        Debug.Log($"보스 남은 HP : {Hp}");
        if (Hp <= 0)
            Die();
    }
    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);

        GameManager.Instance.OnBossEnemyKilled();
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        StartCoroutine(DieRoutine());
      
   


    }
}
