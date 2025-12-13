using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public GameObject settingsPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel == null) return;

            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}
