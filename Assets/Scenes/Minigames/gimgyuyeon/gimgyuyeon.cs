using UnityEngine;

public class Shooter : MonoBehaviour
{
    // �ν����Ϳ��� �巡���ؼ� ���� ���� ������
    public GameObject bubblePrefab;

    // ������ ���ư��� �ӵ�
    public float shootForce = 10f;

    void Update()
    {
        // ���콺 Ŭ�� �� ����
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ���� ���
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            // ���� ����
            GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

            // ������ �ӵ� �ο�
            Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * shootForce;
            }
        }
    }
}
