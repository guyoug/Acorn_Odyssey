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
            shootEnemy();
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            StopAllEnemySpawn();
            //몸통 박치기 이후에 그거 코루틴 끝나면 실행하는걸로
        }
    }
    void shootEnemy()
    {
        Debug.Log("shootEnemy 실행됨");

        if (player == null)
        {
            Debug.LogError("player가 NULL임!!!");
            return;
        }
        if (firePoint == null)
        {
            Debug.LogError("firePoint가 NULL임!!!");
            return;
        }
        if (enemyPrefabs == null)
        {
            Debug.LogError("enemyPrefabs가 NULL임!!!");
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
    void StopAllEnemySpawn() //스폰시 enemy 스폰 X
    {
        GameObject spawnObj = GameObject.FindGameObjectWithTag("Enemy_Spawn_Manager");
        EnemySpawn2 spawner = spawnObj.GetComponent<EnemySpawn2>();
        spawner.StopSpawn();
    }

}
