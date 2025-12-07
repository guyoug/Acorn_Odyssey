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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);   // ★ Player 유지
    }
    void Start()
    {
        Health();
        UpdateUI();
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
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Game Over");
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

}



