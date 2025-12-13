using UnityEngine;

public class Stage2_Boss : MonoBehaviour
{
    [Header("Boss Status")]
    public int Hp = 60;
    private bool isDead = false;
    private GameManager gameManager;
    [Header("Boss Shoot Points")]
    public Transform firePointCenter;
    public Transform firePointUp;
    public Transform firePointDown;

    [Header("Shoot Prefab")]
    public GameObject bulletPrefab;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        
    }
}
