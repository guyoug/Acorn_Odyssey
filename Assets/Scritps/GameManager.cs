using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 1;
    public int bossinterval = 5;      
    public int spawnedCount = 0;     

    [Header("Kill Count")]
    public int normalKilled = 0;
    public int eliteKilled = 0;
    public int bossKilled = 0;

    [Header("Boss Spawn Control")]
    private bool bossSpawned = false;

    [Header("Enemy Prefabs")]
    public GameObject elitePrefabs;
    public GameObject bossPrefabs;

    [Header("Spawn Points")]
    public Transform eliteSpawnPoint;
    public Transform bossSpawnPoint;

    [Header("UI Panels")]
    public GameObject stageClearPanel;

    [Header("Singleton")]
    public static GameManager Instance;


private void Awake() //싱글턴
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        StartWave(currentWave);
    }

    void Update()
    {
        switch (currentWave) //엘리트 스폰
        {
            case 2:
                if (currentWave == 2 && normalKilled == 8 && spawnedCount == 0) 
                    SpawnElite();
                break;

            case 3:
                if (currentWave == 3 && normalKilled >= 11 && spawnedCount == 0 || (currentWave == 3 && normalKilled >= 22 && spawnedCount == 1))
                    SpawnElite();
                break;

        }

        if (Input.GetKeyDown(KeyCode.F4)) // 스테이지 넘기는 치트
        {
            OnBossEnemyKilled();
        }
        if(Input.GetKeyDown(KeyCode.F5)) // 보스 소환하는 차트
        {
            SpawnBoss();
        }
    }

    public void OnNormalEnemyKilled() //기본 킬  확인
    {
        normalKilled++;
        Debug.Log("기본 몬스터  제거 : " + normalKilled);
        CheckWave();
    }
    public void OnEliteEnemyKilled() //엘리트 킬 확인
    {
        eliteKilled++;
        Debug.Log("엘리트 몬스터 제거 : " + eliteKilled);
        CheckWave();
    }
    public void OnBossEnemyKilled() // 보스 킬 확인
    {
        bossKilled++;
        Debug.Log("보스 제거 : " + bossKilled);
        StartCoroutine(killboss());
      
    }
    public void SpawnElite() // 엘리트 스폰
    {
        Instantiate(elitePrefabs, eliteSpawnPoint.transform.position, eliteSpawnPoint.transform.rotation);
        spawnedCount++;
    }
    public void CheckWave() // 웨이브 넘기기
    {
        if (currentWave == 1 && normalKilled >= 12)
        {
            StartNextWave();
        }
        if (currentWave == 2 && normalKilled >= 16 && eliteKilled >= 1)
        {
            StartNextWave();
        }
        if (currentWave == 3 && normalKilled >= 22 && eliteKilled >= 2)
        {
            SpawnBoss();
        }
    }
    public void StartNextWave() //웨이브 시초기화
    {
        currentWave++;
        normalKilled = 0;
        eliteKilled = 0;
        bossKilled = 0;
        spawnedCount = 0;
        StartWave(currentWave);
    }
    void StartWave(int wave)
    {
        Debug.Log($"Wave {wave} 시작!");
        
    }
    void SpawnBoss() // 보스 스폰
    {
        if (bossSpawned)
            return;
        Instantiate(bossPrefabs, bossSpawnPoint.position, bossSpawnPoint.rotation);
        bossSpawned = true;
        StopAllEnemySpawn();
    }
    void StopAllEnemySpawn() //스폰시 enemy 스폰 X
    {
        GameObject spawnObj = GameObject.FindGameObjectWithTag("Enemy_Spawn_Manager");
        EnemySpawn spawner = spawnObj.GetComponent<EnemySpawn>();
        spawner.StopSpawn();
    }

    IEnumerator killboss() //보스 죽으면 
    {
        stageClearPanel.SetActive(true);
        yield return new WaitForSeconds(bossinterval);
        stageClearPanel.SetActive(false);
        SceneManager.LoadScene("Game_Play_stage2");
    }
}