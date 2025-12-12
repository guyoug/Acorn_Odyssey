using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class Elite2WarningBlink : MonoBehaviour
{
    [Header("Warning Image")]
    public Image warnImg;

    [Header("Blink Settings")]
    public float blinkTime = 0.2f;
    public int blinkCount = 3;

    public IEnumerator Blink()
    {
        if (warnImg == null)
        {
            Debug.LogError("warnImg가 연결되지 않았습니다.");
            yield break;
        }

        gameObject.SetActive(true);

        for (int i = 0; i < blinkCount; i++)
        {
            warnImg.enabled = true;
            yield return new WaitForSeconds(blinkTime);

            warnImg.enabled = false;
            yield return new WaitForSeconds(blinkTime);
        }

        gameObject.SetActive(false);
    }
}