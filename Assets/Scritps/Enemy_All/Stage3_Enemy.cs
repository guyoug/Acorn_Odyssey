using UnityEngine;

public class Enemy3 : MonoBehaviour
{
   
    public int maxspeed = 20;
    public bool isDead = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * maxspeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Outline"))
        {
            Die();
        }
    }
    public void Die()
    {
        if (isDead)
            return;
        isDead = true;

        gameManager.OnNormalEnemyKilled(); // 킬 계산

        Destroy(gameObject);
    }
}

