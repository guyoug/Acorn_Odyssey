using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;
    public bool isInvincible = false; // 무적 여부

    [Header("UI Elements")]
    public Image[] Player_HP;

    [Header("Hit Flash")]
    public float hitFlashTime = 0.1f;
    private Coroutine hitFlashRoutine;
    private SpriteRenderer sr;

    public GameObject gameOverPanel;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);  
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "Game_Start")
        {
            Instance = null;  
            Destroy(gameObject);
           
        }

        GameObject healthPanel = GameObject.Find("Health_Panel");
        if (healthPanel == null)
        {
            Debug.LogError("Health_Panel 못 찾음");
            return;
        }

        Player_HP = healthPanel.GetComponentsInChildren<Image>(false);

        ResetState();
    }
    void Start()
    {
        Health();
        UpdateUI();
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        // 무적 토글 치트
        if (Input.GetKeyDown(KeyCode.P))
        {
            isInvincible = !isInvincible;   // ← 핵심
            Debug.Log(isInvincible ? "무적 ON" : "무적 OFF");
        }
    }
    void HideStageCanvas()
    {
        GameObject stageCanvas =
            GameObject.FindGameObjectWithTag("StageCanvas");

        if (stageCanvas != null)
            stageCanvas.SetActive(false);
    }
    void Health() // 최대치
    {
        currentHealth = maxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 0 ~ maxHealth 
    }
    
    void UpdateUI()
    {

        if (Player_HP == null)
            return;

        for (int i = 0; i < Player_HP.Length; i++)
            Player_HP[i].enabled = i < currentHealth; // 체력 ui
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return; //무적이면 무시

        if (isDead)
           return;

        currentHealth -= damage;
        UpdateUI();

        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);
        hitFlashRoutine = StartCoroutine(HitFlash());
        SoundManager.Instance.PlaySFX(SoundManager.Instance.playerHitSFX);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead)
            return;
        isDead = true;
        currentHealth = 0;
        UpdateUI();
        HideStageCanvas();
        GameManager.Instance.ShowGameOver();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.gameOverSFX);
     
        Time.timeScale = 0f;
    }

    public void Heal(int heal)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
        UpdateUI();
    }
    IEnumerator HitFlash()
    {
        if (sr == null)
            yield break;

        sr.color = new Color(1f, 0.4f, 0.4f, 1f);
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = Color.white;
    }

    public void ResetState()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateUI();
    }
}



