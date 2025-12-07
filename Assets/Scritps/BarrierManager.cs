using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    [Header("Barrier Rotation Settings")]
    private float rotateSpeed = 180.0f; 
    private float globalAngle = 0f;     

    [Header("Barrier Prefabs & Units")]
    public GameObject barrierPrefab;
    public List<BarrierUnit> barrierList = new List<BarrierUnit>(); 
    void Update()
    {
        RotateAllBarriers(); // 밤송이 회전
    }
    void RotateAllBarriers()
    {
        if (barrierList.Count == 0) 
            return;
        globalAngle += rotateSpeed * Time.deltaTime; // 원 회전
        float angleStep = 360f / barrierList.Count; //갯수에 따라 균등한 각도
        for (int i = 0; i < barrierList.Count; i++)
        { 
            barrierList[i].UpdatePosition(globalAngle + angleStep * i); //밤송이 각도 업데이트
        }
    }
    public void AddBarrier() //밤송이 추가
    {
        if (barrierList.Count >= 3)
        {
            Debug.Log("밤송이는 최대 3개까지 생성됩니다.");
            return;
        }
        GameObject obj = Instantiate(barrierPrefab); //위치
        obj.transform.SetParent(transform);
        BarrierUnit unit = obj.GetComponent<BarrierUnit>();        // BarrierUnit 스크립트를 가져와 리스트에 추가
        barrierList.Add(unit);
    }
    public void RemoveBarrier(BarrierUnit unit)  // 밤송이 제거
    {
        barrierList.Remove(unit);
    }

}
