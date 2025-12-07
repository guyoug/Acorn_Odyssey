using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public int Speed = 10;
    public float SpeedStack = 0.25f;
    private float X;
    private float Y;

    [Header("References")]
    private PlayerUpgrade upgrade;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        upgrade = GetComponent<PlayerUpgrade>();
    }

    void Update()
    {
         X = Input.GetAxisRaw("Horizontal");
         Y = Input.GetAxisRaw("Vertical");
    }

     void FixedUpdate()
    {
        float maxSpeed = Speed + (upgrade.speedStack * SpeedStack);
        Vector3 position = rb.position;
        position = new Vector3(position.x + (X * maxSpeed * Time.fixedDeltaTime), position.y + (Y * maxSpeed * Time.fixedDeltaTime), position.z);
        rb.MovePosition(position);

    }
}
