using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip mainBGM;
    public AudioClip stage1BGM;
    public AudioClip stage2BGM;
    public AudioClip stage3BGM;
    public AudioClip enemyDieSFX;
    public AudioClip stageClearSFX;
    public AudioClip playerHitSFX;
    public AudioClip gameOverSFX;
    public AudioClip pickPowerupSFX;    

    private bool isMasterMuted = false;
    private float prevBGMVolume = 0.6f;
    private float prevSFXVolume = 0.8f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        bgmSource.volume = prevBGMVolume;
        sfxSource.volume = prevSFXVolume;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMByScene(scene.name);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayBGMByScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Game_Start":
                PlayBGM(mainBGM);
                break;

            case "Game_Play_stage1":
                PlayBGM(stage1BGM);
                break;

            case "Game_Play_stage2":
                PlayBGM(stage2BGM);
                break;

            case "Game_Play_stage3":
                PlayBGM(stage3BGM);
                break;

            default:
                break;
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }
    public void ResumeBGM()
    {
        if (bgmSource == null || bgmSource.clip == null) return;
        bgmSource.Play();
    }
    public void PlayBGMForce(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void ToggleMasterMute()
    {
        if (!isMasterMuted)
        {
            prevBGMVolume = bgmSource.volume;
            prevSFXVolume = sfxSource.volume;

            bgmSource.volume = 0f;
            sfxSource.volume = 0f;
            isMasterMuted = true;
        }
        else
        {
            bgmSource.volume = prevBGMVolume;
            sfxSource.volume = prevSFXVolume;
            isMasterMuted = false;
        }

    }
    public void UnmuteIfNeeded()
    {
        if (!isMasterMuted)
            return;

        bgmSource.volume = prevBGMVolume;
        sfxSource.volume = prevSFXVolume;
        isMasterMuted = false;
    }

    public void SetBGMVolume(float value)
    {
        UnmuteIfNeeded();
        bgmSource.volume = value;
        prevBGMVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        UnmuteIfNeeded();
        sfxSource.volume = value;
        prevSFXVolume = value;
    }
    public void PlayMainBGM()
    {
        if (bgmSource == null || mainBGM == null)
            return;

        bgmSource.Stop();              // 이전 상태 전부 무시
        bgmSource.clip = mainBGM;
        bgmSource.loop = true;

        // 볼륨 복구 (0이면 기본값)
        if (bgmSource.volume <= 0f)
            bgmSource.volume = prevBGMVolume > 0f ? prevBGMVolume : 0.6f;

        isMasterMuted = false;         // 전체 뮤트 상태였다면 해제
        bgmSource.Play();
    }
}