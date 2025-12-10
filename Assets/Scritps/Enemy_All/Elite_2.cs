using System.Collections;
using UnityEngine;

public class Elite_2 : MonoBehaviour
{
    [Header("Movement Range (Y Axis)")]
    private float minY = -2.0f;
    private float maxY = 3.3f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private bool movingUp = true;

    [Header("References")]
    private Rigidbody2D rb;
    public GameObject enemyPrefabs;
    public Transform firePoint;
    public Transform player;
    public EliteWarningBlink warningUI;

    [Header("Fire Timing")]

    public float fireDelay = 1.5f;
    private float timer = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
  
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireDelay)
        {
            timer = 0f;
            StartCoroutine(ShootWithWarning());
        }

        //if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    StopAllEnemySpawn();
            
        //}
    }
    IEnumerator ShootWithWarning()
    {
        // 🔥 먼저 경고 UI 깜빡이기
        if (warningUI != null)
            yield return StartCoroutine(warningUI.Blink());

        // 🔥 깜빡임 끝나면 총알 발사
        shootEnemy();
    }
    void shootEnemy()
    {
        Debug.Log("shootEnemy");

        if (player == null)
        {
            Debug.LogError("player null");
            return;
        }

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
    void StopAllEnemySpawn()
    {
        GameObject spawnObj = GameObject.FindGameObjectWithTag("Enemy_Spawn_Manager");
        EnemySpawn2 spawner = spawnObj.GetComponent<EnemySpawn2>();
        spawner.StopSpawn();
    }

}
