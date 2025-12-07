using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [Header("Speed Upgrade")]
    public int speedStack = 0;
    public int maxSpeedStack = 5;

    [Header("Multi-Shot Upgrade")]
    public int moreStack = 0;
    public int maxMoreStack = 3;
    public int basicShotCount = 1;

    [Header("NutStorm Upgrade")]
    public int nutStack = 0;
    public int maxNutStack = 2;

    [Header("Tail Settings")]
    public int maxTailCount = 4;
    public GameObject tailPrefab;
    public List<GameObject> tailList = new List<GameObject>();

    [Header("References")]
    private PlayerShoot shoot;
    private BarrierManager barrierManager;
    void Start()
    {
        shoot = GetComponent<PlayerShoot>();
        barrierManager = GetComponent<BarrierManager>();
    }
    public void ActivateSpeedUp()
    {
        if (speedStack < maxSpeedStack)
        {
            speedStack++;
            Debug.Log($"Speed Stack : {speedStack} ");
        }
        else
        {
            Debug.Log("스피드 최대 스택!");
        }
    
    }
    public void ActivateMore()
    {
        if (moreStack < maxMoreStack)
        {
            moreStack++;
            Debug.Log($"MORE 강화! 현재 스택: {moreStack}");

        }
        else
        {
            Debug.Log("MORE 최대 스택!");
        }
        int multiShotCount = basicShotCount + moreStack * 1; //총알 수 = 기본 1발 + 스택당 1발
        if (shoot != null)
            shoot.multiShot = multiShotCount;
    }
    public void ActivateNutStorm() //넛스톰
    {

        shoot.burstCount = 3 + nutStack * 3;
        shoot.isNutStormMode = true;

        // 나중에 스택 증가
        if (nutStack < maxNutStack)
            nutStack++;
    

    } 
    public void ActivateBarrier() //밤송이
    {
        Debug.Log("밤송이 추가!");
        barrierManager.AddBarrier();

    }
  
    public void ActivateTail()
    {
        if (tailList.Count >= maxTailCount)
        {
            Debug.Log("Tail 최대 수(4기) 도달");
            return;
        }
        GameObject newTail = Instantiate(tailPrefab);
        Tail tail = newTail.GetComponent<Tail>();
        tail.player = this.transform;

        
        int index = tailList.Count;
        float xOffset = -2.5f;
        float yOffset = (index - 1.5f) * 0.7f;

        tail.offset = new Vector3(xOffset, yOffset, 0);

        tailList.Add(newTail);

        Debug.Log($"Tail 생성 완료! 현재 Tail 수: {tailList.Count}");
    }
}

