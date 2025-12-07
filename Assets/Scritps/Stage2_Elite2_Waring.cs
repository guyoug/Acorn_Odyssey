using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelBlink : MonoBehaviour
{
    public Image panelImage;
    public float minAlpha = 40f / 255f;
    public float maxAlpha = 114f / 255f;
    public float duration = 2f;

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        Color c = panelImage.color;

        while (true)
        {
            // 투명 → 불투명
            yield return StartCoroutine(Fade(minAlpha, maxAlpha));

            // 불투명 → 투명
            yield return StartCoroutine(Fade(maxAlpha, minAlpha));
        }
    }

    IEnumerator Fade(float start, float end)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            float newAlpha = Mathf.Lerp(start, end, t);

            Color c = panelImage.color;
            c.a = newAlpha;
            panelImage.color = c;

            yield return null;
        }
    }
}