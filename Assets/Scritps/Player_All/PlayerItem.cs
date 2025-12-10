using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerItem : MonoBehaviour
{
    [Header("Item Settings")]
    private float itv = 0.3f;
    private int currentItem = 0;
    public bool hasItem1 = false; // 썬더넛 보유 여부
    public bool hasItem2 = false; // 데스넛 소스 보유 여부
    public bool hasItem3 = false; // 청량 탄산수 도토리 향 보유 여부

    [Header("Attack Prefabs")]
    public GameObject conePrefab;
    public GameObject laserPrefab;

    [Header("Fire Point")]
    public Transform firePoint;

    [Header("Item UI Elements")]
    public GameObject item1Image;
    public GameObject item2Image;
    public GameObject item3Image;

    [Header("Item Activation UI")]
    public GameObject itemUI1;
    public GameObject itemUI2;
    public GameObject itemUI3;

    [Header("Item Attribute UI")]
    public GameObject attrUI1;
    public GameObject attrUI2;
    public GameObject attrUI3;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (currentItem == 1) UseItem1();
            else if (currentItem == 2) UseItem2();
            else if (currentItem == 3) UseItem3();
        }


        //치트
        if (Input.GetKeyDown(KeyCode.F1))
            PickItem(1);

        if (Input.GetKeyDown(KeyCode.F2))
            PickItem(2);

        if (Input.GetKeyDown(KeyCode.F3))
            PickItem(3);

    }

    void ClearAllItems()
    {
        hasItem1 = false;
        hasItem2 = false;
        hasItem3 = false;

        itemUI1.SetActive(false);
        itemUI2.SetActive(false);
        itemUI3.SetActive(false);

        attrUI1.SetActive(false);
        attrUI2.SetActive(false);
        attrUI3.SetActive(false);

        item1Image.SetActive(false);
        item2Image.SetActive(false);
        item3Image.SetActive(false);
    }

    public void PickItem(int type)
    {
        ClearAllItems();
        currentItem = type;

        switch (type)
        {
            case 1:
                hasItem1 = true;
                itemUI1.SetActive(true);    // 보유 UI
                attrUI1.SetActive(true);    // 속성 UI
                break;

            case 2:
                hasItem2 = true;
                itemUI2.SetActive(true);
                attrUI2.SetActive(true);
                break;

            case 3:
                hasItem3 = true;
                itemUI3.SetActive(true);
                attrUI3.SetActive(true);
                break;
        }

        Debug.Log($"아이템 {type}번 획득! (기존 아이템 삭제 + 속성 UI 적용)");
    }
    // 1번 아이템: 썬더넛 → 화면의 모든 EnemyBullet 제거
    void UseItem1()
    {
        StartCoroutine(ShowItem1());
        Debug.Log("1번 아이템 발동");
        foreach (var bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
            Destroy(bullet);
        hasItem1 = false;
        itemUI1.SetActive(false);
        attrUI1.SetActive(false);
        currentItem = 0;
    }
    // 2번 아이템: 데스넛 → cone 발사
    void UseItem2()
    {
        StartCoroutine(ShowItem2());
        Debug.Log("2번 아이템 발동");
        FireCone();
        hasItem2 = false;
        itemUI2.SetActive(false);
        attrUI2.SetActive(false);
        currentItem = 0;
    }
    // 3번 아이템: 탄산 도토리 → 레이저 발사
    void UseItem3()
    {
        StartCoroutine(ShowItem3());
        Debug.Log("3번 아이템 발동");
        WaterLaser();
        hasItem3 = false;
        itemUI3.SetActive(false);
        attrUI3.SetActive(false);
        currentItem = 0;
    }
    IEnumerator ShowItem1()
    {
        item1Image.SetActive(true);
        yield return new WaitForSeconds(itv);
        item1Image.SetActive(false);
    }
    IEnumerator ShowItem2()
    {
        item2Image.SetActive(true);
        yield return new WaitForSeconds(itv);
        item2Image.SetActive(false);
    }
    IEnumerator ShowItem3()
    {
        item3Image.SetActive(true);
        yield return new WaitForSeconds(itv);
        item3Image.SetActive(false);
    }
    void FireCone()
    {
        GameObject obj = Instantiate(conePrefab, firePoint.position, firePoint.rotation);

        FireCone cone = obj.GetComponent<FireCone>();
        cone.target = this.transform;
        Instantiate(conePrefab, firePoint.position, firePoint.rotation);
    }

    void WaterLaser()
    {
        Vector3 pos = new Vector3(0f, firePoint.position.y, firePoint.position.z);
        Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
    }
}

