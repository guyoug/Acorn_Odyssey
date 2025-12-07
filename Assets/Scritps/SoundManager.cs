using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;

    [Header("BGM Clips")]
    public AudioClip mainBGM;
    public AudioClip stage1BGM;
    public AudioClip stage2BGM;
    public AudioClip stage3BGM;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
}
