using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 60;
    private bool isDead = false;
    public float maxSpeed = 2f;
    private GameManager gameManager;

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

    [Header("Movement Settings (Elite 방식)")]
    private float minX = 3.5f;
    private float maxX = 7.0f;
    private float minY = -2.0f;
    private float maxY = 3.3f;
    private Vector3 targetPos;

    [Header("Death Sprite")]
    public Sprite deadSprite;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;
    private float DeadSprite = 0.3f;

    [Header("Hit Flash")]
    private Coroutine hitFlashRoutine;
    void Start()
    {
        gameManager = GameManager.Instance;
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        SetNewTarget();
        StartCoroutine(PattenLoop());
    }
    private void FixedUpdate()
    {
        MoveRandom();
        
    }

    IEnumerator PattenLoop()
    {
        while (true)
        {
            if (isDead)
                yield break;

            yield return StartCoroutine(BurstPattern());
            yield return StartCoroutine(KnifePattern());
            yield return new WaitForSeconds(GroupDelay);
        }
    }
    IEnumerator BurstPattern()
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
        for (int i = 0; i < 3; i++)
        {
            Instantiate(knifePrefab, throwPoint.position, throwPoint.rotation);
            yield return new WaitForSeconds(knifeDelay);
        }
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
            anim.enabled = false;
            sr.sprite = deadSprite;
        }


        if (col != null)
            col.enabled = false;

        maxSpeed = 0f;


        yield return new WaitForSeconds(DeadSprite);

        Destroy(gameObject);
    }
    void MoveRandom()
    {
        if (isDead)
            return;
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, maxSpeed * Time.deltaTime);
        transform.position = newPos;


        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            SetNewTarget();
        }
    }
    void SetNewTarget()
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
            hitFlashRoutine = StartCoroutine(HitFlash());
            Destroy(collision.gameObject);
        }
    }
    public void TakeDamage(int dmg)
    {
        if (isDead) 
            return;
        Hp -= dmg;
        Debug.Log($"보스 남은 HP : {Hp}");
        if (Hp <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) 
            return;
        isDead = true;
        if (gameManager != null)
            gameManager.OnBossEnemyKilled();
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        StartCoroutine(DieRoutine());

    }
  
}
