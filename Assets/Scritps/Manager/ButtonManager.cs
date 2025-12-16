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
    private float prevVolume = 1f;
    private bool isSFXMuted = false;
    private float prevSFXVolume = 1f;

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

        PlayerHealth.Instance.ResetState();

        PlayerGauge gauge =
            PlayerHealth.Instance.GetComponent<PlayerGauge>();
        gauge.ResetState();

        PlayerUpgrade upgrade =
            PlayerHealth.Instance.GetComponent<PlayerUpgrade>();
        upgrade.ResetState();

        GameManager.Instance.ResetState(); // 필요하면

        SceneManager.LoadScene("Game_Play_stage1");
    }


    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        if (SoundManager.Instance == null)
            return;

        bgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
        bgmSlider.value = SoundManager.Instance.bgmSource.volume;
        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);

        isBGMMuted = bgmSlider.value <= 0f;
        UpdateBGMIcon();

        sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
        sfxSlider.value = SoundManager.Instance.sfxSource.volume;
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        isSFXMuted = sfxSlider.value <= 0f;
        UpdateSFXIcon();
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
        if (SoundManager.Instance == null || SoundManager.Instance.bgmSource == null)
            return;

        var bgm = SoundManager.Instance.bgmSource;

        if (!isBGMMuted)
        {
            // 현재 볼륨 저장하고 0으로
            prevVolume = bgm.volume;
            bgm.volume = 0f;

            // 슬라이더도 0으로 내려가도록
            bgmSlider.value = 0f;
            isBGMMuted = true;
        }
        else
        {
            // 이전 볼륨 복원
            bgm.volume = prevVolume;

            // 슬라이더도 원래대로 복원
            bgmSlider.value = prevVolume;
            isBGMMuted = false;
        }
        UpdateBGMIcon();
    }
    public void MuteSFX()
    {
        if (SoundManager.Instance == null || SoundManager.Instance.sfxSource == null)
            return;

        var sfx = SoundManager.Instance.sfxSource;

        if (!isSFXMuted)
        {
            // 현재 볼륨 저장
            prevSFXVolume = sfx.volume;

            // 음소거
            sfx.volume = 0f;

            // 슬라이더도 0으로
            if (sfxSlider != null)
                sfxSlider.value = 0f;

            isSFXMuted = true;
        }
        else
        {
            // 볼륨 복원
            sfx.volume = prevSFXVolume;

            // 슬라이더도 복원
            if (sfxSlider != null)
                sfxSlider.value = prevSFXVolume;

            isSFXMuted = false;
        }
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


