using System.Collections;
using UnityEngine;

public class Stage2_Elite : MonoBehaviour
{
    [Header("Settings")]
    public int Hp = 15;
    private bool isDead = false;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private bool movingUp = true;
    private float minY = -2.0f;
    private float maxY = 3.3f;

    [Header("Attack Settings")]
    public float fireDelay = 1.5f;
    private float fireTimer = 0f;
    private bool isPatternRunning = false;

    [Header("References")]
    public GameObject enemyPrefabs;
    public Transform firePoint;
    public Elite2WarningBlink warningUI;
    private GameManager gameManager;
    public GameObject[] dropItems;

    [Header("Dash Pattern")]
    public float dashSpeed = 8f;
    public float returnSpeed = 4f;
    public GameObject exclamationUI;  
    private Vector3 spawnPosition;


    void Start()
    {
        gameManager = GameManager.Instance;
   

    }
    void Update()
    {
        if (isDead || isPatternRunning)
            return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireDelay)
        {
            fireTimer = 0f;
            StartCoroutine(ShootPatternWithWarning());
        }
    }

    IEnumerator ShootPatternWithWarning()
    {
        isPatternRunning = true;

        if (warningUI != null)
            yield return StartCoroutine(warningUI.Blink());

        ShootEnemy();

        isPatternRunning = false;
    }


void ShootEnemy()
    {
        Debug.Log("shootEnemy");

        Instantiate(enemyPrefabs, firePoint.position, firePoint.rotation);
    }
    private void FixedUpdate()
    {
        Move();
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


