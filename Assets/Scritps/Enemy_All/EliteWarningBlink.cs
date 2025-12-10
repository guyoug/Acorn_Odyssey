using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EliteWarningBlink : MonoBehaviour
{
    public Image warnImg;        // 깜빡일 이미지
    public float blinkTime = 0.2f;   // 1회 깜빡이는 시간
    public int blinkCount = 3;       // 총 몇 번 깜빡일지

    public IEnumerator Blink()
    {
        if (warnImg == null)
            yield break;

        // 이미지 켜둔다
        warnImg.gameObject.SetActive(true);

        for (int i = 0; i < blinkCount; i++)
        {
            warnImg.enabled = true;
            yield return new WaitForSeconds(blinkTime);

            warnImg.enabled = false;
            yield return new WaitForSeconds(blinkTime);
        }

        warnImg.gameObject.SetActive(false);
    }
}
    
