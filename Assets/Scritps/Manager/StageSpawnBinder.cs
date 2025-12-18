using UnityEngine;
public class StageSpawnBinder: MonoBehaviour
{

    public Transform eliteSpawn;
    public Transform bossSpawn;
  
    private void Awake()
    {
        GameManager.Instance.BindSpawnPoints(eliteSpawn, bossSpawn);

    }
}
