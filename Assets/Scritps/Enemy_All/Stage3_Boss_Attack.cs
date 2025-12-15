using UnityEngine;

public class Stage3_Boss_Attack : MonoBehaviour
{

    public int Damage = 1;
    public int maxspeed = 10;
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * maxspeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(Damage);
        }
    }
}

