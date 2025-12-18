using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingLoopMove : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 3f;

    [Header("Loop Settings")]
    public float resetX = -10f; 
    public float startX = 10f;    

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

       
        if (transform.position.x <= resetX)
        {
            Vector3 pos = transform.position;
            pos.x = startX;
            transform.position = pos;
        }

        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Game_Start");
        }
    }
}





