using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Attack Timing")]
    public float delayTime = 3.0f;
    public float checkTime = 0.0f;

    [Header("Attack Prefabs & Points")]
    public GameObject bulletPrefabs;
    public Transform firePoint;

    [Header("References")]
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (firePoint == null)
        {
            Debug.Log("firePoint이 null입니다.");
            return;
        }
        if (bulletPrefabs == null)
        {
            Debug.Log("bulletPrefabs이 null입니다.");
            return;
        }
           
    }
    void Update()
    {
        checkTime += Time.deltaTime;
        if (checkTime >= delayTime)
        {
            checkTime = 0f;
            Shoot();
        }
    }
    void Shoot()
    {
        if (player == null)
        {
           Debug.Log ("player가 null입니다.");
            return;
        }
        Vector3 dir = (player.position - firePoint.position).normalized; // 방향 계산
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // Atan2를 이용해 회전 각도 계산
        firePoint.rotation = Quaternion.Euler(0, 0, angle+ +180f);    // 실제 발사 방향으로 firePoint 회전 (여기서 +180f는 탄환 prefab의 기본 방향 보정)
        Instantiate(bulletPrefabs, firePoint.position, firePoint.rotation);
    }
}