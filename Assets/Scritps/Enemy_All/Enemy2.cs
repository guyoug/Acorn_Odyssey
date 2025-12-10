using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header("Status")]
    public int maxspeed = 15;
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
    private void Start()
    {
        Destroy(gameObject, deleteTime);
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * maxspeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Die();
        }
        if (collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }
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
