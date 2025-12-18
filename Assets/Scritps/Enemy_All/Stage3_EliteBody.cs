using System.Collections;
using UnityEngine;

public class Stage3_EliteBody : MonoBehaviour
{

    private SpriteRenderer sr;
    public Sprite deadSprite;
    private int destroyDelay = 2;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void ChangeToDead()
    {
        if (sr != null && deadSprite != null)
            sr.sprite = deadSprite;
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}

