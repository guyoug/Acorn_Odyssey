using System.Collections;
using UnityEngine;
using TMPro;

public class BlinkTMPText : MonoBehaviour
{
    [Header("Target")]
    public TMP_Text target;

    [Header("Blink Settings")]
    public float interval = 0.8f;  
    public bool startOnEnable = true;

    Coroutine co;

    void OnEnable()
    {
        if (startOnEnable) StartBlink();
    }

    void OnDisable()
    {
        StopBlink(true);
    }

    public void StartBlink()
    {
        if (target == null) target = GetComponent<TMP_Text>();
        if (target == null) return;

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(Blink());
    }

    public void StopBlink(bool showAtEnd = true)
    {
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
        if (target != null) target.enabled = showAtEnd;
    }

    IEnumerator Blink()
    {
        while (true)
        {
            target.enabled = !target.enabled;
            yield return new WaitForSeconds(interval);
        }
    }
}