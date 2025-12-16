using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("Settings")]
    private float interval = 1.0f;
    private bool isPaused = false;
    private bool isBGMMuted = false;
    private bool isSFXMuted = false;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject settingsPanel;


    [Header("BGM UI")]
    public Image bgmIcon;
    public Sprite bgmOnSprite;
    public Sprite bgmOffSprite;
    public Slider bgmSlider;

    [Header("SFX UI")]
    public Image sfxIcon;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;
    public Slider sfxSlider;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (pausePanel == null)
            pausePanel = GameObject.Find("PausePanel");

        if (settingsPanel == null)
            settingsPanel = GameObject.Find("SettingsPanel");

        if (gameOverPanel == null)
            gameOverPanel = GameObject.Find("GameOverPanel");
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

        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            pausePanel = canvas.transform.Find("PausePanel")?.gameObject;
            settingsPanel = canvas.transform.Find("SettingsPanel")?.gameObject;
            gameOverPanel = canvas.transform.Find("GameOverPanel")?.gameObject;
        }
    }
    void Update()
    {
        // 게임 오버 중에는 ESC 무시
        if (gameOverPanel != null && gameOverPanel.activeSelf)
            return;

        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        // 1. 설정창 열려 있으면  설정 닫고 퍼즈 패널로 복귀
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            CloseSettings();   // settings OFF, pause ON
            return;
        }

        // 2. 퍼즈 상태면  다시 시작
        if (isPaused)
        {
            Resume();
            return;
        }

        // 3. 그 외  퍼즈 진입
        Pause();
    }
    public void Pause()
    {
        Debug.Log("pausePanel = " + pausePanel);

        if (isPaused)
            return;
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }
    public void Resume()
    {
        if (!isPaused)
            return;
        StartCoroutine(resume());
    }
    private IEnumerator resume()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        isPaused = false;
        yield return new WaitForSecondsRealtime(interval);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        SoundManager.Instance?.PlayMainBGM();
        SceneManager.LoadScene("Game_Start");
    }

    public void restart()
    {
        gameOverPanel?.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(RestartRoutine());
    }
    IEnumerator RestartRoutine()
    {
        SceneManager.LoadScene("Game_Play_stage1");

        yield return null; // 1 프레임 대기

        PlayerHealth.Instance.ResetState();

        PlayerGauge gauge = PlayerHealth.Instance.GetComponent<PlayerGauge>();
        gauge.ResetState();

        PlayerUpgrade upgrade = PlayerHealth.Instance.GetComponent<PlayerUpgrade>();
        upgrade.ResetState();

        PlayerItem item = PlayerHealth.Instance.GetComponent<PlayerItem>();

        item.ClearAllItems();

        GameManager.Instance.ResetState();
    }


    public void OpenSettings()
    {
        if (SoundManager.Instance == null)
           return;

        settingsPanel.SetActive(true);

        bgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);

        bgmSlider.value = SoundManager.Instance.bgmSource.volume;
        sfxSlider.value = SoundManager.Instance.sfxSource.volume;

        isBGMMuted = bgmSlider.value <= 0f;
        isSFXMuted = sfxSlider.value <= 0f;

        UpdateBGMIcon();
        UpdateSFXIcon();

        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
       
    }


    public void OnBGMSliderChanged(float value)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.SetBGMVolume(value);

        isBGMMuted = value <= 0f;
        UpdateBGMIcon();
    }
    public void OnSFXSliderChanged(float value)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.SetSFXVolume(value);

        isSFXMuted = value <= 0f;
        UpdateSFXIcon();
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void MuteBGM()
    {
        SoundManager.Instance.ToggleBGMMute();
        bgmSlider.value = SoundManager.Instance.bgmSource.volume;
        isBGMMuted = bgmSlider.value <= 0f;
        UpdateBGMIcon();
    }
    public void MuteSFX()
    {
        SoundManager.Instance.ToggleSFXMute();
        sfxSlider.value = SoundManager.Instance.sfxSource.volume;
        isSFXMuted = sfxSlider.value <= 0f;
        UpdateSFXIcon();

    }
    void UpdateBGMIcon()
    {
        if (bgmIcon == null) return;

        bgmIcon.sprite = isBGMMuted
            ? bgmOffSprite
            : bgmOnSprite;
    }
    void UpdateSFXIcon()
    {
        if (sfxIcon == null) return;

        sfxIcon.sprite = isSFXMuted
            ? sfxOffSprite
            : sfxOnSprite;
    }
}


