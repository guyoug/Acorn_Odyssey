using System.Collections;
using UnityEngine;

public class EnemySpawn1 : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemy1Prefabs;
    public int interval = 2;

    [Header("Spawn Range (Y Axis)")]
    public float maxY = 3.3f;
    public float minY = -3.1f;

    [Header("Runtime")]
    private Coroutine spawnRoutine;
    void Start()
    {
        spawnRoutine = StartCoroutine(spawnEnemy());
    }
    public void StopSpawn()  // GameManager에서 보스 소환시 멈추는 코드
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }
    IEnumerator spawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Vector3 position = new Vector3(transform.position.x, Random.Range(minY, maxY), transform.position.z); // 3.3 ~ -3.1 사이 랜덤 소환
            Instantiate(enemy1Prefabs, position, transform.rotation);
        }
    }
}
