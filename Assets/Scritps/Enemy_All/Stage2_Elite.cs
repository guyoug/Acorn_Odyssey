using System.Collections;
using UnityEngine;

public class Elite_2 : MonoBehaviour
{
    [Header("Settings")]
    public int Hp = 15;
    private bool isDead = false;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private bool movingUp = true;
    private float minY = -2.0f;
    private float maxY = 3.3f;

    [Header("References")]

    public GameObject enemyPrefabs;
    public Transform firePoint;
    public Transform player;
    public Elite2WarningBlink warningUI;
    private GameManager gameManager;
    public GameObject[] dropItems;


    [Header("Fire Timing")]

    public float fireDelay = 1.5f;


    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        gameManager = GameManager.Instance;
        StartCoroutine(SpawnSequence());

    }

    IEnumerator SpawnSequence()
    {
        while (true)
        {
            if (isDead)
                yield break;
            if (warningUI != null)
                yield return StartCoroutine(warningUI.Blink());
            shootEnemy();
            yield return new WaitForSeconds(1.0f);
        }
    }
    void shootEnemy()
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
    //void StopAllEnemySpawn()
    //{
    //    GameObject spawnObj = GameObject.FindGameObjectWithTag("Enemy_Spawn_Manager");
    //    EnemySpawn2 spawner = spawnObj.GetComponent<EnemySpawn2>();
    //    spawner.StopSpawn();
    //}


