using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    public float speed = 2f;

    private Transform bg1;
    private Transform bg2;
    private float width;

    void Start()
    {
        // 첫 번째 배경 = 현재 오브젝트
        bg1 = transform;

        // 배경의 가로 길이 계산
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;

        // 두 번째 배경 자동 생성 + 오른쪽에 붙이기
        bg2 = Instantiate(bg1, new Vector3(bg1.position.x - width, bg1.position.y, bg1.position.z), Quaternion.identity);

        // BG2만 180도 플립 (Y축 반전)
        bg2.localScale = new Vector3(bg2.localScale.x, -bg2.localScale.y, bg2.localScale.z);

        bg1.name = "BG1";
        bg2.name = "BG2_Flipped";
    }

    void Update()
    {
        // 오른쪽으로 이동
        bg1.Translate(Vector3.right * speed * Time.deltaTime);
        bg2.Translate(Vector3.right * speed * Time.deltaTime);

        // BG1이 오른쪽 화면을 벗어나면 BG2 왼쪽 뒤로
        if (bg1.position.x >= width)
        {
            bg1.position = new Vector3(bg2.position.x - width, bg1.position.y, bg1.position.z);
        }

        // BG2도 동일하게 처리
        if (bg2.position.x >= width)
        {
            bg2.position = new Vector3(bg1.position.x - width, bg2.position.y, bg2.position.z);
        }
    }
}