using System.Collections;
using UnityEngine;

public class Stage2_Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 60;
    private bool isDead = false;
    private GameManager gameManager;

    [Header("Boss Shoot Points")]
    public Transform firePointCenter;
    public Transform firePointUp;
    public Transform firePointDown;


    [Header("Warning UI")]
    public Boss2WaringBlink centerWarning;
    public Boss2WaringBlink upWarning;
    public Boss2WaringBlink downWarning;

    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public float shootInterval = 0.3f;
    public float patternDelay = 1f;

    [Header("Shoot Angles")]
    public float centerAngle = 0f;   
    public float topAngle = 24f;      
    public float bottomAngle = -30f;   
    void Start()
    {
        gameManager = GameManager.Instance;
        StartCoroutine(BossShootPattern());
    }

    IEnumerator BossShootPattern()
    {
        yield return new WaitForSeconds(3f);

        while (!isDead)
        {
            
            if (centerWarning != null)
                yield return StartCoroutine(centerWarning.Blink());

            yield return StartCoroutine(
                ShootBurst(firePointCenter, 5, centerAngle)
            );

            yield return new WaitForSeconds(patternDelay);

            // 2️⃣ 위 (↙)
            if (upWarning != null)
                yield return StartCoroutine(upWarning.Blink());

            yield return StartCoroutine(
                ShootBurst(firePointUp, 3, topAngle)
            );

            yield return new WaitForSeconds(patternDelay);

            // 3️⃣ 아래 (↖)
            if (downWarning != null)
                yield return StartCoroutine(downWarning.Blink());

            yield return StartCoroutine(
                ShootBurst(firePointDown, 3, bottomAngle)
            );
        }
    }


    IEnumerator ShootBurst(Transform firePoint, int count, float angle)
    {
        if (firePoint == null)
            yield break;

        Quaternion rot = Quaternion.Euler(0, 0, angle );


        for (int i = 0; i < count; i++)
        {
            Instantiate(bulletPrefab, firePoint.position, rot);
            yield return new WaitForSeconds(shootInterval);
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
        Debug.Log($"보스 남은 HP : {Hp}");
        if (Hp <= 0)
            Die();
    }
    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        if (gameManager != null)
            gameManager.OnBossEnemyKilled();
        StopAllCoroutines();
        Destroy(gameObject);


    }
}
