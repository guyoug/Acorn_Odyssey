using System.Collections;
using UnityEngine;

public class Stage3_Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 200;
    private bool isDead = false;
    private float deathDelay = 0.2f;
    
    [Header("Death Sprite")]
    public Sprite deadSprite;
    [Header("References")]
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    [Header("Whip")]
    public GameObject whip;

    [Header("Whip Points (1 → 2 → 3)")]
    public Transform point1;
    public Transform point2;
    public Transform point3;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float whipActiveTime = 0.2f;

    [Header("Runtime")]
    private Coroutine hitFlashRoutine;

    public Sprite normalSprite;      
    public Sprite HitSprite;      



    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        whip.SetActive(false);
        StartCoroutine(WhipPattern());
    }

    IEnumerator WhipPattern()
    {
        while (true)
        {
            //포인트 1 = 위 2 = 가운데 3 = 아래
            yield return StartCoroutine(MoveAndHit(point1));
            yield return StartCoroutine(MoveAndHit(point2));
            yield return StartCoroutine(MoveAndHit(point3));

            
            yield return new WaitForSeconds(1f);
            whip.SetActive(true);
            StartCoroutine(ShootSpriteEffect());
            yield return new WaitForSeconds(whipActiveTime);
            whip.SetActive(false);

         
            yield return StartCoroutine(MoveAndHit(point2));
            yield return StartCoroutine(MoveAndHit(point1));
        }
    }
    IEnumerator ShootSpriteEffect()
    {
        bool prevAnimEnabled = false;
        if (anim != null)
        {
            prevAnimEnabled = anim.enabled;
            anim.enabled = false; // 
        }

        sr.sprite = HitSprite;
        yield return new WaitForSeconds(whipActiveTime);
        sr.sprite = normalSprite;

        if (anim != null)
            anim.enabled = prevAnimEnabled;
    }
    IEnumerator MoveAndHit(Transform target)
    {
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

        // 도착 → whip 활성 (타격)
        whip.SetActive(true);
        StartCoroutine(ShootSpriteEffect());
        yield return new WaitForSeconds(whipActiveTime);
        whip.SetActive(false);
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

        if (GameManager.Instance != null)
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

        StopAllCoroutines();

        Destroy(gameObject);
    }
}
