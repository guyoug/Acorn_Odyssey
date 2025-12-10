using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;

public class Tail : MonoBehaviour
{
    public float followSpeed = 8f;
    [Header("References")]
    public Transform player;
    private PlayerShoot playerShoot;

    [Header("Fire Point")]
    public Transform firePoint;

    [Header("Follow Settings")]
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerShoot = player.GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer();
     
    }
    public void followPlayer()
    {

        Vector3 target = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
    }
    public void FireByPlayer(bool isNutStorm)
    {
        if (isNutStorm)
        {
            StartCoroutine(BurstCopy());
        }
        else
        {
            FireCopyShot();
        }
    }
    IEnumerator BurstCopy()
    {
        for (int i = 0; i < playerShoot.burstCount; i++)
        {
            FireCopyShot();
            yield return new WaitForSeconds(playerShoot.brustDelay);
        }
    }
    void FireCopyShot()
    {
        int count = playerShoot.multiShot;
        float space = playerShoot.space;
        float spaceX2 = playerShoot.spaceX2;

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = Vector3.zero;

            if (count == 2)
            {
                if (i == 0) offset = new Vector3(0, space, 0);
                if (i == 1) offset = Vector3.zero;
            }
            else if (count == 3)
            {
                if (i == 0) offset = new Vector3(0, space, 0);
                if (i == 1) offset = Vector3.zero;
                if (i == 2) offset = new Vector3(0, -space, 0);
            }
            else if (count == 4)
            {
                if (i == 0) offset = new Vector3(0, spaceX2, 0);
                if (i == 1) offset = new Vector3(0, space, 0);
                if (i == 2) offset = Vector3.zero;
                if (i == 3) offset = new Vector3(0, -space, 0);
            }

            Instantiate(
                playerShoot.bulletPrefabs,
                firePoint.position + offset,
                firePoint.rotation
            );
        }
    }
}
