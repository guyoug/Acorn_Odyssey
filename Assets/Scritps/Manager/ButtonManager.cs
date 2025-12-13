using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("Settings")]
    private float interval = 1.0f;
    private bool isPaused = false;
    private bool isMuted = false;
    private float prevVolume = 1f;
    private bool isSFXMuted = false;
    private float prevSFXVolume = 1f;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject settingsPanel;


    [Header("Sound UI")]
    public Slider bgmSlider;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(settingsPanel.activeSelf)
            {
                CloseSettings();
                return;
            }

            if (!isPaused)
            {
                Pause();
                return;
            }
        }
    }
    public void GameStart()
    {
        SceneManager.LoadScene("Game_Play_stage1");
    }
    public void GameQuit()
    {
        Application.Quit();
        Debug.Log("게임종료");//위 코드 유니티 에디터에선 동작 X > 디버그로 확인
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
        SceneManager.LoadScene("Game_Start");
    }

    public void restart()
    {
        gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGMForce(SoundManager.Instance.stage1BGM);



        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.ResetPlayer();

            PlayerGauge gauge = PlayerHealth.Instance.GetComponent<PlayerGauge>();
            if (gauge != null)
                gauge.ResetGauge();

            PlayerUpgrade upgrade = PlayerHealth.Instance.GetComponent<PlayerUpgrade>();
            if (upgrade != null)
                upgrade.ResetUpgrade();
        }

        SceneManager.LoadScene("Game_Play_stage1");
    }
    public void OpenSettings()
    {

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        
        if (SoundManager.Instance != null)
        {
            //BGM
            bgmSlider.onValueChanged.RemoveListener(OnBGMSliderChanged);
            bgmSlider.value = SoundManager.Instance.bgmSource.volume;
            bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
            //SFX
            sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
            sfxSlider.value = SoundManager.Instance.sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }
    }
    
    public void OnBGMSliderChanged(float value)
    {
        if (SoundManager.Instance == null || SoundManager.Instance.bgmSource == null)
        {
            Debug.LogWarning("SoundManager 또는 BGM Source가 없습니다!");
            return;
        }

        SoundManager.Instance.bgmSource.volume = value;
    }
    public void OnSFXSliderChanged(float value)
    {
        if (SoundManager.Instance == null || SoundManager.Instance.sfxSource == null)
            return;

        SoundManager.Instance.sfxSource.volume = value;

        // 슬라이더 움직이면 뮤트 해제
        if (value > 0f)
            isSFXMuted = false;
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

        if (!isMuted)
        {
            // 현재 볼륨 저장하고 0으로
            prevVolume = bgm.volume;
            bgm.volume = 0f;

            // 슬라이더도 0으로 내려가도록
            bgmSlider.value = 0f;
            isMuted = true;
        }
        else
        {
            // 이전 볼륨 복원
            bgm.volume = prevVolume;

            // 슬라이더도 원래대로 복원
            bgmSlider.value = prevVolume;
            isMuted = false;
        }
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
    }
}


