using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stage1_Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 60;                 
    public float maxSpeed = 2f;        
    private bool isDead = false;        

    [Header("Pattern Points")]
    public Transform firePoint;
    public Transform throwPoint;

    [Header("Pattern Prefabs")]
    public GameObject bulletPrefab;
    public GameObject knifePrefab;

    [Header("Boss Attack Settings")]
    public float burstDelay = 0.15f; 
    public float burstGroupDelay = 0.5f;
    public float knifeDelay = 0.7f;
    public float GroupDelay = 0.8f;

    [Header("Movement Settings")]
    private float minX = 3.5f;
    private float maxX = 7.0f;
    private float minY = -2.0f;
    private float maxY = 3.3f;

    [Header("Runtime")]
    private Vector3 targetPos;

    [Header("Death Sprite")]
    public Sprite deadSprite;               
    private float DeadSpritetime = 1.5f;
    public Sprite BoomSprite;
    private float boomSprite = 0.5f;

    [Header("References")]
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    [Header("Hit Flash")]
    private Coroutine hitFlashRoutine;
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        SetNewTarget();
        StartCoroutine(PattenLoop());
    }
    private void FixedUpdate()
    {
        MoveRandom(); //랜덤 위치로 계속 움직임
        
    }

    IEnumerator PattenLoop()
    {
        while (!isDead)
        {
          
            yield return StartCoroutine(BurstPattern());
            yield return StartCoroutine(KnifePattern());
            yield return new WaitForSeconds(GroupDelay);
        }
    }
    IEnumerator BurstPattern() // 총 3번씩 3발 점사
    {
        for (int group = 0; group < 3; group++)
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                yield return new WaitForSeconds(burstDelay);
            }

            yield return new WaitForSeconds(burstGroupDelay);
        }
    }
    IEnumerator KnifePattern()
    {
        for (int i = 0; i < 3; i++) // 칼 세 번 던짐
        {
            Instantiate(knifePrefab, throwPoint.position, throwPoint.rotation);
            yield return new WaitForSeconds(knifeDelay);
        }
    }
    void SetNewTarget()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        targetPos = new Vector3(x, y, transform.position.z);
    }
    void MoveRandom()
    {
        if (isDead)
            return; 
        //tragetpos로 이동
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, maxSpeed * Time.deltaTime);
        transform.position = newPos;

        // 목표 위치에 거의 도달하면 새로운 목표 설정  
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            SetNewTarget();
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
        hitFlashRoutine = StartCoroutine(HitFlash());
   
        Debug.Log($"보스 남은 HP : {Hp}");
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
        StopCoroutine(PattenLoop());
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);
        GameManager.Instance.OnBossEnemyKilled();
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

        maxSpeed = 0f;

        yield return new WaitForSeconds(DeadSpritetime);

        sr.sprite = BoomSprite;

        yield return new WaitForSeconds(boomSprite);

        Destroy(gameObject);
    }

}
