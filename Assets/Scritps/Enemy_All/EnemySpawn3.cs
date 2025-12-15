using System.Collections;
using UnityEngine;

public class EnemySpawn3 : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemy3Prefabs;
    public float interval = 2.0f;

    [Header("Spawn Range (Y Axis)")]
    public float maxY = 3.3f;
    public float minY = -3.1f;

    [Header("Runtime")]
    private Coroutine spawnRoutine;

    //스폰 위치 2개
    public Transform[] spawnPoints;
    void Start()
    {
    

        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(spawnEnemy());

    }
    public void StopSpawn()
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
            int rand = Random.Range(0, spawnPoints.Length);
            Transform point = spawnPoints[rand];

            Instantiate(enemy3Prefabs, point.position, point.rotation);
            
        }
    }
}