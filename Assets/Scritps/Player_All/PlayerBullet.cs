using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class PlayerBullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 20f;

    [Header("References")]
    private Rigidbody2D rb;
    //private Vector2 shootDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        MoveBullet();
    }
    private void MoveBullet()
    {
        Vector3 position = rb.transform.position;
        position = new Vector3(position.x + (+1 * maxSpeed * Time.deltaTime), position.y, position.z);
        rb.MovePosition(position);
        //rb.linearVelocity = shootDirection * maxSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")|| collision.CompareTag("Elite")|| collision.CompareTag("Outline"))
        {
            Destroy(gameObject);
        }
    }
}

