using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingLoopMove : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 3f;

    [Header("Loop Settings")]
    public float resetX = -10f;   // 이 값보다 왼쪽으로 가면
    public float startX = 10f;    // 이 위치로 돌아감

    void Update()
    {
        // 왼쪽 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 화면 밖으로 나가면 위치 리셋
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





