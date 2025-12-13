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
        bgmSource.volume = 0.6f;
        sfxSource.volume = 0.8f;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMByScene(scene.name);
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
}
