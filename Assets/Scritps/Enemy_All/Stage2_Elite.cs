using System.Collections;
using UnityEngine;

public class Stage2_Elite : MonoBehaviour
{
    [Header("Status")]
    public int Hp = 15;
    private bool isDead = false;
    private bool isPatternRunning = false;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
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

    private Vector3 spawnPosition;

    void Awake()
    {
        gameManager = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (firePoint == null)
            firePoint = transform.Find("FirePoint");

        warningUI = warningUIObj.GetComponent<Elite2WarningBlink>();
        //  if (warningUI == null)
              //warningUI = GetComponentInChildren<Elite2WarningBlink>(true);

        if (exclamationUI == null)
            exclamationUI = transform.Find("ExclamationUI")?.gameObject;

        if (exclamationUI != null)
            exclamationUI.SetActive(false);

        spawnPosition = transform.position;
    }

    void Start()
    {
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

            // 5️ 원위치 복귀
            yield return StartCoroutine(ReturnToSpawn());

            // 6️ 1초 휴식
            yield return new WaitForSeconds(1f);

            isPatternRunning = false;
        }
    }

    void ShootEnemy()
    {
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