using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;
    public bool isInvincible = false; // 무적 여부

    [Header("UI Elements")]
    public Image[] Player_HP;
    public GameObject gameOverPanel;

    [Header("Hit Flash")]
    public float hitFlashTime = 0.1f;
    private Coroutine hitFlashRoutine;
    private SpriteRenderer sr;
    public static PlayerHealth Instance;
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

    void Start()
    {
        Health();
        UpdateUI();
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        // 무적 토글 치트
        if (Input.GetKeyDown(KeyCode.F12))
        {
            isInvincible = false;
        }
    }
    void Health() // 최대치
    {
        currentHealth = maxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 0 ~ maxHealth 
    }
    
    void UpdateUI()
    {
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
        isDead = true;
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.gameOverSFX);

        Debug.Log("Game Over");
        PlayerGauge gauge = GetComponent<PlayerGauge>();
        if (gauge != null)
            gauge.ShowGaugeUI(false); 
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Heal(int heal)
    {
        if (isDead) 
            return;

        currentHealth += heal;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

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

    public void ResetPlayer()
    {
        isDead = false;
        isInvincible = false;
        currentHealth = maxHealth;
        UpdateUI();
    }
}



