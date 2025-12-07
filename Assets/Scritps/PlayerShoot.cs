using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{


    [Header("Shooting Settings")]
    public Transform firePoint;
    public GameObject bulletPrefabs;

    [Header("Shot Parameters")]
    public int burstCount = 1;
    public int multiShot = 1; // more 수
    public float space = 0.7f; // 간격
    public float spaceX2 = 1.4f; // 간격 4번째 때 사용

    [Header("Timing Settings")]
    public float delayTime = 0.3f; // 샷 딜레이
    public float checkTime = 0.0f;
    public float brustDelay = 0.3f; // 넛스톰 연사 간격

    [Header("NutStorm Mode")]
    public bool isNutStormMode = false;
    private bool isBurstRunning = false;

    void Update()
    {
        checkTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (checkTime >= delayTime)
            {
                Shoot();
                checkTime = 0f;
            }

            return;   // <<<<<< 이 한 줄이 핵심
        }
    }


    public void Shoot()
    {
        PlayerUpgrade upgrade = GetComponent<PlayerUpgrade>();

        // 넛스톰 모드일 때
        if (isNutStormMode)
        {
            if (!isBurstRunning)
            {
                Debug.Log("넛스톰 점사 시작!");
                isBurstRunning = true;
                StartCoroutine(NutStormBurst());
            }

            if (upgrade != null)
            {
                foreach (var tailObj in upgrade.tailList)
                {
                    Tail tail = tailObj.GetComponent<Tail>();
                    if (tail != null)
                        tail.FireByPlayer(true);  
                }
            }

            return; 
        }


     
        Shot();

        if (upgrade != null)
        {
            foreach (var tailObj in upgrade.tailList)
            {
                Tail tail = tailObj.GetComponent<Tail>();
                if (tail != null)
                    tail.FireByPlayer(false);
            }
        }
    }
    public void Shot()
    {
        for (int i = 0; i < multiShot; i++)
        {
            Vector3 offset = Vector3.zero;
            //총알 배치
            if (multiShot == 1)
                offset = Vector3.zero;

            else if (multiShot == 2) // 위 가운데 
            {
                if (i == 0)
                    offset = new Vector3(0, space, 0);
                if (i == 1)
                    offset = Vector3.zero;
            }

            else if (multiShot == 3) //위 가운데 아래
            {
                if (i == 0)
                    offset = new Vector3(0, space, 0);
                if (i == 1)
                    offset = Vector3.zero;
                if (i == 2)
                    offset = new Vector3(0, -space, 0);
            }

            else if (multiShot == 4) //위x2 위 가운데 아래
            {
                if (i == 0)
                    offset = new Vector3(0, spaceX2, 0);
                if (i == 1)
                    offset = new Vector3(0, space, 0);
                if (i == 2)
                    offset = Vector3.zero;
                if (i == 3)
                    offset = new Vector3(0, -space, 0);
            }
            Instantiate(bulletPrefabs, firePoint.position + offset, firePoint.rotation);
        }
    }
    IEnumerator NutStormBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            NutStormMoreShot();
            yield return new WaitForSeconds(brustDelay);

        }
        isBurstRunning = false;
    }
    void NutStormMoreShot() //넛스톰 멀티샷
    {
        for (int i = 0; i < multiShot; i++)
        {
            Vector3 offset = Vector3.zero;

            if (multiShot == 1)
            {
                offset = Vector3.zero;
            }
            else if (multiShot == 2)
            {
                if (i == 0) offset = new Vector3(0, space, 0);
                if (i == 1) offset = Vector3.zero;
            }
            else if (multiShot == 3)
            {
                if (i == 0) offset = new Vector3(0, space, 0);
                if (i == 1) offset = Vector3.zero;
                if (i == 2) offset = new Vector3(0, -space, 0);
            }
            else if (multiShot == 4)
            {
                if (i == 0) offset = new Vector3(0, spaceX2, 0);
                if (i == 1) offset = new Vector3(0, space, 0);
                if (i == 2) offset = Vector3.zero;
                if (i == 3) offset = new Vector3(0, -space, 0);
            }

            Instantiate(bulletPrefabs, firePoint.position + offset, firePoint.rotation);
        }
    }
}





