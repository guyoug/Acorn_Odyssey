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
    [Header("Screen Bounds")]
    public float minX = -9.5f;
    public float maxX = 9.5f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        upgrade = GetComponent<PlayerUpgrade>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
            halfHeight = sr.bounds.extents.y;
        }
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
        position.x = Mathf.Clamp(
           position.x,
           minX + halfWidth,
           maxX - halfWidth
       );

        position.y = Mathf.Clamp(
            position.y,
            minY + halfHeight,
            maxY - halfHeight
        );

        rb.MovePosition(position);

    }
}
