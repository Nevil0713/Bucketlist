using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform firePoint;
    public float shootForce = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBubble();
        }
    }

    void ShootBubble()
    {
        if (bubblePrefab == null || firePoint == null) return;

        GameObject bubble = Instantiate(bubblePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = firePoint.up;
            rb.velocity = direction * shootForce;
        }
    }
}
