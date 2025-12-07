using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Status")]
    public int Hp = 5;
    public float maxspeed = 10.0f;
    private bool isDead = false;

    [Header("Drop Rates")]
    private float dropItem = 0.05f;  // 5%
    private float dropGauge = 0.10f; // 10%

    [Header("Lifetime Settings")]
    private float deleteTime = 10.0f;

    [Header("Prefabs & Items")]
    public GameObject[] dropItems;
    public GameObject gaugePrefabs;

    [Header("References")]
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
            Debug.Log("GameManager가 null입니다.");
        Destroy(gameObject, deleteTime);
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * maxspeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
            Die();
        else if (collision.CompareTag("Outline"))
            Destroy(gameObject);
    }
    public void Die()
    {
        if (isDead)
           return;
        isDead = true;
        gameManager.OnNormalEnemyKilled(); // 킬 계산
        TryDropItem(); // 속성 아이템
        TryDropGagueItem();// 게이지 아이템
        Destroy(gameObject);
    }   
    private void TryDropItem()
    {
        if (dropItems == null)
        {
            Debug.Log("dropItems 배열이 null입니다.");
            return;
        }
        if (Random.value <= dropItem) //5퍼
        {
            int idx = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[idx], transform.position, Quaternion.identity);
        }
    }
    private void TryDropGagueItem()
    {
        if (gaugePrefabs == null)
        {
            Debug.Log("gaugePrefabs가 null입니다.");
            return;
        }
        if (Random.value <= dropGauge) //25퍼
            Instantiate(gaugePrefabs, transform.position, Quaternion.identity);
    }
}
