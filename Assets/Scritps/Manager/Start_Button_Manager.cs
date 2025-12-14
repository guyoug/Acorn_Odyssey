using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMuteManager : MonoBehaviour
{
    public Button muteButton;

    public Image muteIcon;

    [Header("Sprites")]
    public Sprite soundOnSprite;   
    public Sprite soundOffSprite; 

    void Start()
    {
        UpdateMuteIcon();
    }
    public void ToggleMasterMute()
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.ToggleMasterMute();
        UpdateMuteIcon();
    }
    void UpdateMuteIcon()
    {
        if (SoundManager.Instance == null || muteIcon == null)
            return;

        bool isMuted =
            SoundManager.Instance.bgmSource.volume <= 0f &&
            SoundManager.Instance.sfxSource.volume <= 0f;

        muteIcon.sprite = isMuted ? soundOffSprite : soundOnSprite;
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
}
