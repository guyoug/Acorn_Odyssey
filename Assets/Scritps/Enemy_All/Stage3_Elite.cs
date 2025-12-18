using System.Collections;
using UnityEngine;

public class Stage3_Elite : MonoBehaviour
{
    public int Hp = 100;
    private bool isDead = false;
    public GameObject[] dropItems;
    private Collider2D col;
    private SpriteRenderer sr;
    private float DeadSprite = 2f;

    [Header("Hit Flash")]
    public Sprite deadSprite;
    private Coroutine hitFlashRoutine;

    [Header("Fire")]
    public GameObject fireBreath;

    [Header("Move Points")]
    public Transform bottomPoint;
    public Transform topPoint;

    [Header("Movement")]
    public float moveSpeed = 15f;

    [Header("Timing (Boss Sync)")]
    public float preFireDelay = 0.25f;
    public float fireTime = 0.35f;
    public float postFireDelay = 0.25f;     
    public float turnDelay = 1.0f;

    [SerializeField] private GameObject bodyPrefab;
    public Vector3 spawnPos = new Vector3(6f, -0.28f, 0f);
    private GameObject bodyInstance;
    private Stage3_EliteBody body;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        fireBreath.SetActive(false);
        bodyInstance = Instantiate(bodyPrefab, spawnPos, Quaternion.identity);
        body = bodyInstance.GetComponent<Stage3_EliteBody>();
        StartCoroutine(FireMovePattern());

    }
    IEnumerator FireMovePattern()
    {
        while (!isDead)
        {
            yield return StartCoroutine(MoveAndFire(bottomPoint));
         
            yield return StartCoroutine(MoveAndFire(topPoint));

        
         
            yield return StartCoroutine(MoveAndFire(bottomPoint));

      
            yield return new WaitForSeconds(turnDelay);
        }


        IEnumerator MoveAndFire(Transform target)
        {
            fireBreath.SetActive(false);

           
            while (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

         
            yield return new WaitForSeconds(preFireDelay);

        
            fireBreath.SetActive(true);
            yield return new WaitForSeconds(fireTime);
            fireBreath.SetActive(false);

      
            yield return new WaitForSeconds(postFireDelay);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead)
            return;

        Hp -= dmg;
        hitFlashRoutine = StartCoroutine(HitFlash());
        Debug.Log($"엘리트 HP: {Hp}");

        if (Hp <= 0)
            Die();
    }

    void Die()
    {
        if (isDead)
            return;
        isDead = true;
        body?.ChangeToDead();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);
        GameManager.Instance.OnEliteEnemyKilled();
        DropItem();
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        StartCoroutine(DieRoutine());
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
            sr.sprite = deadSprite;
        }


        if (col != null)
            col.enabled = false;

        yield return new WaitForSeconds(DeadSprite);

        Destroy(gameObject);
    }
    void DropItem()
    {
        if (dropItems.Length == 0)
            return;

        int idx = Random.Range(0, dropItems.Length);
        Instantiate(dropItems[idx], transform.position, Quaternion.identity);
    }

}
