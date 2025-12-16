using System.Collections;
using UnityEngine;

public class Stage1_Enemy : MonoBehaviour
{

    [Header("Status")]
    public int Hp = 5;
    public float maxspeed = 3.0f;
    private bool isDead = false;
    private float DeadSprite = 0.2f;

    [Header("Drop Rates")]
    private float dropItem = 0.05f;  // 5%
    private float dropGauge = 0.10f; // 10%

    [Header("Lifetime Settings")]
    private float lifeTime = 10.0f;

    [Header("Prefabs & Items")]
    public GameObject[] dropItems;
    public GameObject gaugePrefabs;

    [Header("Death Sprite")]
    public Sprite deadSprite;              

    [Header("Hit Flash")]
    public float hitFlashTime = 0.1f;    
  
    [Header("References")]
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    private Coroutine hitFlashRoutine;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        Destroy(gameObject, lifeTime);
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
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        Hp -= damage;

        hitFlashRoutine = StartCoroutine(HitFlash());

        if (Hp <= 0)
            Die();
    }
    IEnumerator HitFlash()
    {
        if (sr == null || isDead) yield break;

        sr.color = new Color(1f, 0.4f, 0.4f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);
        GameManager.Instance.OnNormalEnemyKilled();
        TryDropItem(); // 속성 아이템
        TryDropGagueItem();// 게이지 아이템
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        StartCoroutine(DieRoutine());
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
