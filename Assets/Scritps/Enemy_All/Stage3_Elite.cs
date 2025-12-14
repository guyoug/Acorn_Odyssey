using System.Collections;
using UnityEngine;

public class Stage3_Elite : MonoBehaviour
{
    public int Hp = 30;
    private bool isDead = false;
    public GameObject[] dropItems;
    private Collider2D col;
    private SpriteRenderer sr;
    private float DeadSprite = 0.3f;

    [Header("Hit Flash")]
    public Sprite deadSprite;
    private Coroutine hitFlashRoutine;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();

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
            //anim.enabled = false;
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

    void Update()
    {
        
    }
}
