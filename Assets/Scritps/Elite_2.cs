using UnityEngine;

public class Elite_2 : MonoBehaviour
{
    private float minY = -2.0f;
    private float maxY = 3.3f;
    public float moveSpeed = 2f;

    private bool movingUp = true;

    void Update()
    {
        Vector3 pos = transform.position;

 
        if (movingUp)
        {
            pos.y += moveSpeed * Time.deltaTime;
            if (pos.y >= maxY)
                movingUp = false;
        }
 
        else
        {
            pos.y -= moveSpeed * Time.deltaTime;
            if (pos.y <= minY)
                movingUp = true;
        }

        transform.position = pos;
    }
}
