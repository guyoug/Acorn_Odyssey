using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerGauge : MonoBehaviour
{
    [Header("Gauge Values")]
    public int gauge = 0;
    public int maxGauge = 5;

    [Header("References")]
    private PlayerUpgrade upgrade;
    private PlayerHealth health;

    [Header("Gauge UI Slots")]
    public Image[] offSlots;
    public Image[] onSlots;

    [Header("Gauge")]
    public GameObject gaugePrefab;
    public GameObject gaugeUIRoot;

    void Awake()
    {
        upgrade = GetComponent<PlayerUpgrade>();
        health = GetComponent<PlayerHealth>();
      
    }
 
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("Game_Play"))
            return;

        StartCoroutine(BindGaugeUI());

    }
    IEnumerator BindGaugeUI()
    {
        yield return null;

        GameObject offRoot = GameObject.Find("OffSlots");
        GameObject onRoot = GameObject.Find("OnSlots");

        if (offRoot == null || onRoot == null)
        {
            Debug.LogError("[GaugeUI] OffSlots / OnSlots 못 찾음");
            yield break;
        }

        offSlots = offRoot.GetComponentsInChildren<Image>(true);
        onSlots = onRoot.GetComponentsInChildren<Image>(true);

        Debug.Log($"[GaugeUI] Bind 완료 → OFF:{offSlots.Length}, ON:{onSlots.Length}");

        ResetState();
    }

  
    public void ShowGaugeUI(bool show)
    {
        if (gaugeUIRoot != null)
            gaugeUIRoot.SetActive(show);
    }
    void Update()
    {   // G 누르면 게이지 값만큼 능력 발동
        if (Input.GetKeyDown(KeyCode.K))
        {
            Activate();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SpawnGaugeCheat();
        }

    }

    public void AddGauge()
    {
        gauge++;

        if (gauge <= maxGauge)
        {
            Debug.Log($"게이지 획득 → 현재: {gauge}");
        }
        if (gauge > maxGauge)
        {
            Debug.Log("게이지 초과! HP 회복 후 초기화!");
            health.Heal(1);
            gauge = 0;
        }
        UpdateGaugeUI();
    }

    void UpdateGaugeUI()
    {
        if (offSlots == null || onSlots == null)
            return;

        int slotCount = Mathf.Min(offSlots.Length, onSlots.Length);
        int safeGauge = Mathf.Clamp(gauge, 0, slotCount);

        // 전체 초기화
        for (int i = 0; i < slotCount; i++)
        {
            offSlots[i].gameObject.SetActive(true);
            offSlots[i].enabled = true;

            onSlots[i].gameObject.SetActive(false);
            onSlots[i].enabled = false;
        }

        // ON 슬롯 켜기
        for (int i = 0; i < safeGauge; i++)
        {
            offSlots[i].gameObject.SetActive(false);

            onSlots[i].gameObject.SetActive(true);   
            onSlots[i].enabled = true;
        }
    }


    void Activate()
    {
        if (gauge == 0)
        {
            Debug.Log("게이지가 부족합니다");
            return;
        }
        Debug.Log("G 버튼 → 게이지 발동: " + gauge);

        switch (gauge)
        {
            case 1:
                upgrade.ActivateSpeedUp(); // 속도 증가 
                break;

            case 2:
                upgrade.ActivateMore(); // 총알 수 증가 (위아래)
                break;

            case 3:
                upgrade.ActivateNutStorm();  // 연사
                break;

            case 4:
                upgrade.ActivateBarrier(); // 실드(밤송이) 생성
                break;

            case 5:
                upgrade.ActivateTail(); //테일(아직미구현)
                break;
        }
        gauge = 0;
        UpdateGaugeUI();
    }
    void SpawnGaugeCheat()
    {
        // 플레이어 바로 앞에 생성
        Vector3 pos = transform.position + new Vector3(1f, 0f, 0f);

        Instantiate(gaugePrefab, pos, Quaternion.identity);
        Debug.Log("치트: 게이지 아이템 생성!");
    }
    public void ResetState()
    {
        gauge = 0;
        UpdateGaugeUI();
    }
}

