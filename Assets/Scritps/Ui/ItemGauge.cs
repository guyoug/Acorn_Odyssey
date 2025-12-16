using UnityEngine;

public class ItemGauge : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 1.5f;
    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerGauge gauge = collision.GetComponent<PlayerGauge>();
            gauge.AddGauge();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.pickPowerupSFX);
            Debug.Log($"게이지 증가!{gauge.gauge}");
            Destroy(gameObject);
        }
        if (collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }
    }
}
