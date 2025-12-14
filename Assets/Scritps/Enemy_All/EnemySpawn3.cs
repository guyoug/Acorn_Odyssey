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
    void Start()
    {
        Debug.Log("EnemySpawn2 Start 호출됨 : " + gameObject.name);

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
            Vector3 position = new Vector3(transform.position.x, Random.Range(minY, maxY), transform.position.z);
            Instantiate(enemy3Prefabs, position, transform.rotation);
            yield return new WaitForSeconds(interval);
        }
    }
}