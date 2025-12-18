using System.Collections;
using UnityEngine;

public class Stage3_Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 200;
    private bool isDead = false;
    private float deathDelay = 2f;
    
    [Header("Death Sprite")]
    public Sprite deadSprite;
    [Header("References")]
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    [Header("Whip")]
    public GameObject whip;
    public float attackDelay = 0.6f;
    private bool prevAnimEnabled = false;

    [Header("Whip Points (1 → 2 → 3)")]
    public Transform point1;
    public Transform point2;
    public Transform point3;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float whipActiveTime = 0.3f;

    [Header("Runtime")]
    private Coroutine hitFlashRoutine;

    public Sprite normalSprite;      
    public Sprite HitSprite;

    public GameObject arrowPrefab;
    public Transform[] attackPoints;
    public float arrowDelay = 0.8f;
    public GameObject rockPrefab;
    public Transform[] rockPoints;
    public float rockDelay = 0.4f;


    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        whip.SetActive(false);
        StartCoroutine(WhipPattern());
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            TakeDamage(210);
        }
    }
    IEnumerator WhipPattern()
    {
        while (!isDead)
        {
            if (isDead)
                yield break;
            yield return StartCoroutine(ShootArrows()); // 화살 3발을 랜덤한 위치에 쏜다.
            yield return new WaitForSeconds(attackDelay);

            yield return StartCoroutine(ThrowRocks()); // 돌을 두 번 랜덤한 위치에 던진다.
            yield return new WaitForSeconds(attackDelay);

            //포인트 1 = 위 2 = 가운데 3 = 아래
            yield return StartCoroutine(MoveAndHit(point1));
            yield return StartCoroutine(MoveAndHit(point2));
            yield return StartCoroutine(MoveAndHit(point3));

            yield return StartCoroutine(ShootArrows()); // 화살 3발을 랜덤한 위치에 쏜다.
            yield return new WaitForSeconds(attackDelay);

            yield return StartCoroutine(ThrowRocks()); // 돌을 두 번 랜덤한 위치에 던진다.
            yield return new WaitForSeconds(attackDelay);

             if (anim != null)
            {
                prevAnimEnabled = anim.enabled;
                anim.enabled = false;
            }
            sr.sprite = HitSprite;
            yield return new WaitForSeconds(0.2f);
            sr.sprite = normalSprite;
            whip.SetActive(true);
            yield return new WaitForSeconds(whipActiveTime);
            whip.SetActive(false);
    
            yield return StartCoroutine(MoveAndHit(point2));
            yield return StartCoroutine(MoveAndHit(point1));
        }
    }

    IEnumerator MoveAndHit(Transform target)
    {
        if (isDead)
            yield break;
        // 이동 중엔 whip 비활성
        whip.SetActive(false);

        while (Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        if (isDead)
            yield break;

        yield return new WaitForSeconds(attackDelay);
       
        if (anim != null)
        {
            prevAnimEnabled = anim.enabled;
            anim.enabled = false; // 
        }
        sr.sprite = HitSprite;
        yield return new WaitForSeconds(0.2f);
        sr.sprite = normalSprite;
        whip.SetActive(true);
        yield return new WaitForSeconds(whipActiveTime);
        whip.SetActive(false);
        
    }
    IEnumerator ShootArrows()
    {
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, attackPoints.Length);
            Transform point = attackPoints[rand];

            Instantiate(arrowPrefab, point.position, point.rotation);
            yield return new WaitForSeconds(arrowDelay);
        }
    }
    IEnumerator ThrowRocks()
    {
        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, rockPoints.Length); 
            Transform point = rockPoints[rand];

            Instantiate(rockPrefab, point.position, point.rotation);
            yield return new WaitForSeconds(rockDelay);
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
        Debug.Log($"보스 남은 HP : {Hp}");
        if (Hp <= 0)
            Die();
    }
    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDieSFX);
        PlayerHealth.Instance.StartCoroutine(PlayerHealth.Instance.SetInvincible(2f));
        GameManager.Instance.OnBossEnemyKilled();
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
            anim.enabled = false;
            sr.sprite = deadSprite;
        }


        if (col != null)
            col.enabled = false;

        moveSpeed = 0f;


        yield return new WaitForSeconds(deathDelay);

      

        Destroy(gameObject);
    }
}
