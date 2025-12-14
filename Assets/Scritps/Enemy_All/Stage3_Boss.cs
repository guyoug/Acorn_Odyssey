using System.Collections;
using UnityEngine;

public class Stage3_Boss : MonoBehaviour
{
    public int Hp = 200;
    private bool isDead = false;
    private float DeadSprite = 0.2f;
    [Header("Death Sprite")]
    public Sprite deadSprite;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    [Header("Hit Flash")]
    private Coroutine hitFlashRoutine;

    void Start()
    {

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

        //moveSpeed = 0f;


        yield return new WaitForSeconds(DeadSprite);

        StopAllCoroutines();

        Destroy(gameObject);
    }
}
