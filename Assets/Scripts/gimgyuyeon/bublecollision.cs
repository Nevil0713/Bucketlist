using UnityEngine;

public class BubbleCollision : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹 ����� "Bubble" �±��� ���� ����
        if (collision.gameObject.CompareTag("Bubble"))
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
