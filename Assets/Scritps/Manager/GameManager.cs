using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GAME Settings")]
    public int currentWave = 1;
    public int bossinterval = 5;      
    public int spawnedCount = 0;
    public int currentStage = 1;

    [Header("Kill Count")]
    public int normalKilled = 0;
    public int eliteKilled = 0;
    public int bossKilled = 0;

    [Header("Boss Spawn Control")]
    private bool bossSpawned = false;

    [Header("Enemy Prefabs")]
    public GameObject elitePrefabs;
    public GameObject bossPrefabs;
    public GameObject elite2Prefabs;
    public GameObject boss2Prefabs;
    public GameObject elite3Prefabs;
    public GameObject boss3Prefabs;

    [Header("Current Spawn Points")]
    private Transform currentEliteSpawnPoint;
    private Transform currentBossSpawnPoint;

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
    public void BindSpawnPoints(Transform elite, Transform boss)
    {
        currentEliteSpawnPoint = elite;
        currentBossSpawnPoint = boss;

        Debug.Log("현재 스테이지 스폰 포인트 바인딩 완료");
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 변경 감지: {scene.name}");


        // 1) stageClearPanel 재할당
        stageClearPanel = GameObject.Find("StageClearPanel");

        Debug.Log("씬 전환 후 GameManager 레퍼런스 갱신 완료");
    }
    void Start()
    {
        StartWave(currentWave);
        DetectStage();

    }
    void DetectStage()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // 예: "Game_Play_stage2" → 마지막 숫자 뽑기
        for (int i = sceneName.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(sceneName[i]))
            {
                currentStage = (int)char.GetNumericValue(sceneName[i]);
                break;
            }
        }

        Debug.Log("현재 스테이지 = " + currentStage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4)) // 스테이지 넘기는 치트
        {
            StartCoroutine(killboss());
        }
        if(Input.GetKeyDown(KeyCode.F5)) // 보스 소환하는 차트
        {
            SpawnBoss();
        }
        if(Input.GetKeyDown (KeyCode.F7))
        {
            SpawnElite();
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SpawnElite2();
        }
    }
    void StageWaveLogic()
    {
        switch (currentStage)
        {
            case 1: Stage1Logic(); break;
            case 2: Stage2Logic(); break;
            case 3: Stage3Logic(); break;
        }
    }
    void Stage1Logic()
    {
        // 엘리트 스폰 규칙
        if (currentWave == 2 && normalKilled >= 20 && spawnedCount == 0 
         || currentWave == 2 && normalKilled >= 40 && spawnedCount == 1
         || currentWave == 2 && normalKilled >= 60 && spawnedCount == 2)
            SpawnElite();
                
        if (currentWave == 3 && normalKilled >= 20 && spawnedCount == 0
         || currentWave == 3 && normalKilled >= 40 && spawnedCount == 1)
            SpawnElite();

        // 웨이브 넘기기 조건
        if (currentWave == 1 && normalKilled >= 30)
            StartNextWave();

        if (currentWave == 2 &&  eliteKilled >= 3)
            StartNextWave();

        if (currentWave == 3 &&  eliteKilled >= 2)
            SpawnBoss();
    }
    void Stage2Logic()
    {

        if (currentWave == 2 && normalKilled >= 30 && spawnedCount == 0 
            || currentWave == 2 && normalKilled >= 60 && spawnedCount == 1)
            SpawnElite2();
        if (currentWave == 3 && normalKilled >= 30 && spawnedCount == 0 
            || currentWave == 3 && normalKilled >= 60 && spawnedCount == 1)
            SpawnElite2();

        if (currentWave == 1 && normalKilled >= 50)
            StartNextWave();
        if (currentWave == 2 && eliteKilled >= 2)
            StartNextWave();

        if (currentWave == 3 && eliteKilled >= 2)
        {
            //SpawnBoss2();
        }

    }

    void Stage3Logic()
    {
        if (currentWave == 1 && normalKilled >= 30)
            StartNextWave();

        if (currentWave == 2 && eliteKilled >= 1)
            StartNextWave();

        if (currentWave == 3 && normalKilled >= 15)
        {
            //SpawnBoss3();
        }

        if (currentWave == 2 && normalKilled >= 15 && spawnedCount == 0)
            SpawnElite();

       
    }
    public void OnNormalEnemyKilled() //기본 킬  확인
    {
        normalKilled++;
        Debug.Log("기본 몬스터  제거 : " + normalKilled);
        StageWaveLogic();
    }
    public void OnEliteEnemyKilled() //엘리트 킬 확인
    {
        eliteKilled++;
        Debug.Log("엘리트 몬스터 제거 : " + eliteKilled);
        StageWaveLogic();

    }
    public void OnBossEnemyKilled() // 보스 킬 확인
    {
        bossKilled++;
        Debug.Log("보스 제거 : " + bossKilled);
        StartCoroutine(killboss());
      
    }
    public void SpawnElite() // 엘리트 스폰
    {
        Instantiate(elitePrefabs, currentEliteSpawnPoint.transform.position, currentEliteSpawnPoint.transform.rotation);
        spawnedCount++;
    }
    public void SpawnElite2()
    {
        Instantiate(elite2Prefabs, currentEliteSpawnPoint.transform.position, currentEliteSpawnPoint.transform.rotation);
        spawnedCount++;
    }
   
    public void StartNextWave() //웨이브 시초기화
    {
        currentWave++;
        normalKilled = 0;
        eliteKilled = 0;
        bossKilled = 0;
        spawnedCount = 0;
        bossSpawned = false;
        StartWave(currentWave);
    }
    void ResetStageData()
    {
        normalKilled = 0;
        eliteKilled = 0;
        bossKilled = 0;
        spawnedCount = 0;
        bossSpawned = false;
    }
    void StartWave(int wave)
    {
        Debug.Log($"Wave {wave} 시작!");

    }
    void SpawnBoss() // 보스 스폰
    {
        if (bossSpawned)
            return;
        Instantiate(bossPrefabs, currentBossSpawnPoint.position, currentBossSpawnPoint.rotation);
        bossSpawned = true;
        StopAllEnemySpawn();
    }
    void SpawnBoss2()
    {
        if (bossSpawned)
            return;
        Instantiate(boss2Prefabs, currentBossSpawnPoint.position, currentBossSpawnPoint.rotation);
        bossSpawned = true;
        StopAllEnemySpawn();
    }
    void SpawnBoss3()
    {
        if (bossSpawned)
            return;

        Instantiate(boss3Prefabs, currentBossSpawnPoint.position, currentBossSpawnPoint.rotation);
        bossSpawned = true;
        StopAllEnemySpawn();
    }
    void StopAllEnemySpawn() //스폰시 enemy 스폰 X
    {
        GameObject spawnObj = GameObject.FindGameObjectWithTag("Enemy_Spawn_Manager");
        EnemySpawn1  spawner = spawnObj.GetComponent<EnemySpawn1>();
        spawner.StopSpawn();
    }

    IEnumerator killboss()
    {
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            stageClearPanel = canvas.transform.Find("StageClearPanel")?.gameObject;
        }

        Debug.Log("killboss 시작, stageClearPanel = " + stageClearPanel);
       
        if (stageClearPanel != null)
            stageClearPanel.SetActive(true);

        PlayerGauge gauge = GameObject.FindWithTag("Player").GetComponent<PlayerGauge>();
        if (gauge != null)
            gauge.ShowGaugeUI(false); // 스테이지 클리어 시 UI 숨김

     

        // timeScale = 0이어도 기다리기 위해 Realtime 사용
        yield return new WaitForSecondsRealtime(bossinterval);

        if (stageClearPanel != null)
            stageClearPanel.SetActive(false);

        if (gauge != null)
            gauge.ShowGaugeUI(true);

        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Game_Play_stage1":
                ResetStageData();
                SceneManager.LoadScene("Game_Play_stage2");
                break;

            case "Game_Play_stage2":
                ResetStageData();
                SceneManager.LoadScene("Game_Play_stage3");
                break;
        }

        //case "Game_Play_stage3":
        //    SceneManager.LoadScene("EndingScene");
        //    break;
    }
}
