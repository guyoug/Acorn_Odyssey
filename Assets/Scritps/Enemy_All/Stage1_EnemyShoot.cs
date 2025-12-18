using UnityEngine;

public class Stage1_EnemyShoot : MonoBehaviour
{
    [Header("Attack Timing")]
    public float delayTime = 3.0f;
    public float checkTime = 0.0f;

    [Header("Attack Prefabs & Points")]
    public GameObject bulletPrefabs;
    public Transform firePoint;

    [Header("References")]
    private Transform player;

    [Header("Shoot Condition")]
    public float stopShootX = -5f;     // 이 X 좌표 이하로 가면 발사 중단


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
        //사격 중단 조건 1: 화면 왼쪽 끝에 갔을때쯔음에
        if (transform.position.x <= stopShootX)
            return;

        //사격 중단 조건 2: 플레이어보다 왼쪽에 있으면 
        if (player != null && transform.position.x <= player.position.x)
            return;

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
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // 방향 벡터에서  회전 각도 변환
        firePoint.rotation = Quaternion.Euler(0, 0, angle+ +180f);    // 실제 발사 방향으로 firePoint 회전 (여기서 +180f는 탄환의 기본 방향 보정)
        Instantiate(bulletPrefabs, firePoint.position, firePoint.rotation);
    }
}