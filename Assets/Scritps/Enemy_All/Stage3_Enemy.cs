using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [Header("Status")]
    public int maxspeed = 20;

    void FixedUpdate()
    {
        transform.Translate(Vector3.left * maxspeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Bullet"))
        //{
        //    Die();
        //}
        if (collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }
    }

}
