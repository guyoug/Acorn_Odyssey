using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [Header("Pause Settings")]
    private float interval = 1.0f;
    private bool isPaused = false;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  
    }
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
           
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
        SceneManager.LoadScene("Game_Start");
    }

    public void restart()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game_Play_stage1");
    }
}

