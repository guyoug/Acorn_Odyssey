using System.Collections;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header("Status")]
    public int Hp = 7;
    public float maxspeed = 15;
    private bool isDead = false;
    private float DeadSprite = 0.2f;

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

    [Header("Death Sprite")]
    public Sprite deadSprite;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    [Header("Hit Flash")]
    private Coroutine hitFlashRoutine;
    private void Start()
    {

        gameManager = GameManager.Instance;
        if (gameManager == null)
            Debug.Log("GameManager가 null입니다.");
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        Destroy(gameObject, deleteTime);
    }
    void FixedUpdate()
    {
        if (isDead)
            return;
        transform.Translate(Vector3.left * maxspeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;
        if (collision.CompareTag("Bullet"))
        {
            Hp--;
            hitFlashRoutine = StartCoroutine(HitFlash());
            Destroy(collision.gameObject);

           
            if (Hp <= 0)
            {
                Die();
            }
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
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);
        gameManager.OnNormalEnemyKilled(); // 킬 계산
        TryDropItem(); // 속성 아이템
        TryDropGagueItem();// 게이지 아이템
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        StartCoroutine(DieRoutine());
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
    IEnumerator HitFlash()
    {
        if (sr == null || isDead) yield break;

        sr.color = new Color(1f, 0.4f, 0.4f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    IEnumerator DieRoutine()
    {
        if (sr != null && deadSprite != null)
        {
            sr.color = Color.white;
            anim.enabled = false;
            sr.sprite = deadSprite;
        }


        if (col != null)
            col.enabled = false;

        maxspeed = 0f;


        yield return new WaitForSeconds(DeadSprite);

        Destroy(gameObject);
    }

}
