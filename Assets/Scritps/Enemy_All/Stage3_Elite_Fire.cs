using UnityEngine;

public class Stage3_Elite_Fire : MonoBehaviour
{
    public int Damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(Damage);
        }
    }
}
       

