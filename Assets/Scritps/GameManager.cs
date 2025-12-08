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

        // 2) 새 씬의 스폰 포인트 재할당
        eliteSpawnPoint = GameObject.Find("EliteSpawnPoint")?.transform;
        bossSpawnPoint = GameObject.Find("BossSpawnPoint")?.transform;

        // 3) EnemySpawn 참조 재설정
        GameObject spawnObj = GameObject.FindGameObjectWithTag("Enemy_Spawn_Manager");
     

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
        if (currentWave == 2 && normalKilled >= 8 && spawnedCount == 0)
            SpawnElite();

        if (currentWave == 3 && normalKilled >= 11 && spawnedCount == 0)
            SpawnElite();

        // 웨이브 넘기기 조건
        if (currentWave == 1 && normalKilled >= 12)
            StartNextWave();

        if (currentWave == 2 && normalKilled >= 16 && eliteKilled >= 1)
            StartNextWave();

        if (currentWave == 3 && normalKilled >= 22 && eliteKilled >= 2)
            SpawnBoss();
    }
    void Stage2Logic()
    {
        
        if (currentWave == 1 && normalKilled >= 10 && spawnedCount == 0)
            SpawnElite();

        if (currentWave == 2 && normalKilled >= 20 && spawnedCount == 1)
            SpawnElite();

        // 웨이브 조건
        if (currentWave == 1 && normalKilled >= 15)
            StartNextWave();

        if (currentWave == 2 && normalKilled >= 25 && eliteKilled >= 1)
            StartNextWave();

        if (currentWave == 3 && normalKilled >= 30 && eliteKilled >= 2)
        {
            //SpawnBoss();
        }

    }

    void Stage3Logic()
    {
        if (currentWave == 1 && normalKilled >= 12 && spawnedCount == 0)
            SpawnElite();

        if (currentWave == 2 && normalKilled >= 18 && spawnedCount == 1)
            SpawnElite();

        if (currentWave == 3 && normalKilled >= 30 && eliteKilled >= 2)
            //SpawnBoss();

        if (currentWave == 1 && normalKilled >= 15)
            StartNextWave();

        if (currentWave == 2 && normalKilled >= 25)
            StartNextWave();
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
        Instantiate(elitePrefabs, eliteSpawnPoint.transform.position, eliteSpawnPoint.transform.rotation);
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
                SceneManager.LoadScene("Game_Play_stage2");
                break;

            case "Game_Play_stage2":
                SceneManager.LoadScene("Game_Play_stage3");
                break;
        }

        //case "Game_Play_stage3":
        //    SceneManager.LoadScene("EndingScene");
        //    break;
    }
}
