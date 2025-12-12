using UnityEngine;
public class StageSpawnBinder: MonoBehaviour
{

    public Transform eliteSpawn;
    public Transform bossSpawn;
    void Start()
    {
        GameManager.Instance.BindSpawnPoints(eliteSpawn, bossSpawn);
    }
}
