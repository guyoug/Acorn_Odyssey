using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    public float speed = 2f;

    public float scrollSpeed = 2f;   // 배경 이동 속도
    public Transform bg1;
    public Transform bg2;

    private float bgWidth;  // 배경 한 장의 가로 길이

    void Start()
    {
        // 배경 한 장의 실제 너비 계산 (SpriteRenderer 기준)
        SpriteRenderer sr = bg1.GetComponent<SpriteRenderer>();
        bgWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // 배경 이동 (왼쪽으로 흐르는 연출)
        bg1.position += Vector3.left * scrollSpeed * Time.deltaTime;
        bg2.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // bg1이 완전히 화면 왼쪽으로 벗어났으면 bg2 뒤로 이동
        if (bg1.position.x <= -bgWidth)
        {
            bg1.position = new Vector3(bg2.position.x + bgWidth, bg1.position.y, bg1.position.z);
        }

        // bg2가 완전히 화면 왼쪽으로 벗어났으면 bg1 뒤로 이동
        if (bg2.position.x <= -bgWidth)
        {
            bg2.position = new Vector3(bg1.position.x + bgWidth, bg2.position.y, bg2.position.z);
        }
    }
}
