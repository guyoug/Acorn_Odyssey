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

    [Header("SFX Clips")]
    public AudioClip enemyDieSFX;
    public AudioClip stageClearSFX;
    public AudioClip playerHitSFX;
    public AudioClip gameOverSFX;
    public AudioClip pickPowerupSFX;
    public AudioClip BarrierHitSFX;

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
        bgmSource.volume = value;

        if (value > 0f)
        {
            prevBGMVolume = value;
            isMasterMuted = false;
        }
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;

        if (value > 0f)
        {
            prevSFXVolume = value;
            isMasterMuted = false;
        }
    }
    public void ToggleBGMMute()
    {
        if (bgmSource.volume > 0f)
        {
            prevBGMVolume = bgmSource.volume;
            bgmSource.volume = 0f;
        }
        else
        {
            bgmSource.volume = prevBGMVolume;
        }
    }
    public void ToggleSFXMute()
    {
        if (sfxSource.volume > 0f)
        {
            prevSFXVolume = sfxSource.volume;
            sfxSource.volume = 0f;
        }
        else
        {
            sfxSource.volume = prevSFXVolume;
        }
    }
    public void PlayMainBGM()
    {
        if (bgmSource == null || mainBGM == null)
            return;

        bgmSource.Stop();            
        bgmSource.clip = mainBGM;
        bgmSource.loop = true;

        if (bgmSource.volume <= 0f)
            bgmSource.volume = prevBGMVolume > 0f ? prevBGMVolume : 0.6f;

        isMasterMuted = false;        
        bgmSource.Play();
    }
    public void ResetState()
    {
        isMasterMuted = false;
        bgmSource.Stop();
        PlayBGMForce(stage1BGM);
    }
}