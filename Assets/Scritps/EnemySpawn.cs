using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefabs;
    public float interval = 1.0f;

    [Header("Spawn Range (Y Axis)")]
    public float maxY = 3.3f;
    public float minY = -3.1f;

    [Header("Runtime")]
    private Coroutine spawnRoutine;
    void Start()
    {
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
            Vector3 position = new Vector3(transform.position.x, Random.Range(minY, maxY), transform.position.z);
            Instantiate(enemyPrefabs, position, transform.rotation);
        }
    }
}
