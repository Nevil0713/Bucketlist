using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Color[] bubbleColors;
    public float shootForce = 500f;

    private GameObject previewBubble;
    private int shootCount = 0;  // 발사 횟수 카운트

    // 버블 매니저 오브젝트를 드래그해서 연결해줘
    public Transform bubbleManagerTransform;

    void Start()
    {
        CreatePreviewBubble();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBubble();
            CreatePreviewBubble();
        }
    }

    void ShootBubble()
    {
        if (previewBubble == null) return;

        previewBubble.GetComponent<Rigidbody2D>().isKinematic = false;
        previewBubble.GetComponent<Rigidbody2D>().AddForce(GetShootDirection() * shootForce);

        shootCount++;  // 발사횟수 증가

        // 7번 발사하면 한 칸 아래로 이동
        if (shootCount >= 7)
        {
            shootCount = 0; // 초기화
            MoveBubblesDown();
        }

        previewBubble = null;
    }

    void CreatePreviewBubble()
    {
        Vector3 spawnPos = transform.position;
        previewBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
        previewBubble.GetComponent<Rigidbody2D>().isKinematic = true;
        previewBubble.GetComponent<SpriteRenderer>().color = bubbleColors[Random.Range(0, bubbleColors.Length)];
    }

    Vector2 GetShootDirection()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
        return (worldPos - transform.position).normalized;
    }

    void MoveBubblesDown()
    {
        // bubbleManagerTransform을 아래로 한 칸 (예: 0.5 단위) 이동
        bubbleManagerTransform.position += new Vector3(0, -0.5f, 0);
    }
}

