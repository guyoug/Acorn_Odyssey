using UnityEngine;

public class ItemGauge : MonoBehaviour
{
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
    }
}
