using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 60;
    private bool isDead = false;
    public float moveSpeed = 2f;
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
    private float gunTimer = 0f;
    public float gunDelay = 2f;
    private float knifeTimer = 0f;
    public float knifeDelay = 0.7f;
    public float ThrowDelay = 4f;  
    private bool isAttacking = false;

    [Header("Movement Settings (Elite 방식)")]
    private float minX = 3.5f;
    private float maxX = 7.0f;
    private float minY = -2.0f;
    private float maxY = 3.3f;
    private Vector3 targetPos;

    void Start()
    {
        gameManager = GameManager.Instance;
        SetNewTarget();
    }
    void Update()
    {
        MoveRandom();

        if (isAttacking)
            return;
        gunTimer += Time.deltaTime;
        knifeTimer += Time.deltaTime;

        
        if (gunTimer >= gunDelay)
        {
            gunTimer = 0f;
            StartCoroutine(BurstPattern());
        }

        // 칼 패턴
        if (knifeTimer >= ThrowDelay)
        {
            knifeTimer = 0f;
            StartCoroutine(KnifePattern());
        }
    }
     IEnumerator KnifePattern()
    {
        while (!isDead)
        {
            yield return StartCoroutine(ThrowKnifePattern());
            yield return new WaitForSeconds(ThrowDelay);
            Debug.Log("한글 깨짐 오류 깃허브용 테스트 코드");
            
        }
    }

    IEnumerator ThrowKnifePattern()
    {
        for (int i = 0; i < 3; i++)
        {
            ThrowKnife();
            yield return new WaitForSeconds(knifeDelay);
        }
    }

    void ThrowKnife()
    {
        Instantiate(knifePrefab, throwPoint.position, throwPoint.rotation);
    }

    IEnumerator BurstPattern()
    {
        isAttacking = true;
        for (int group = 0; group < 3; group++)
        {
            for (int i = 0; i < 3; i++)   
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                yield return new WaitForSeconds(burstDelay);
            }

            yield return new WaitForSeconds(burstGroupDelay);
        }

        isAttacking = false;  
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
        Destroy(gameObject);

    }
    void MoveRandom()
    {
        if (isDead)
            return;
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
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
}
