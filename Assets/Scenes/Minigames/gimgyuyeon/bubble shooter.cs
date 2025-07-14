using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float shootForce = 500f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시 발사
        {
            ShootBubble();
        }
    }

    void ShootBubble()
    {
        Vector3 spawnPos = transform.position;
        GameObject newBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - spawnPos).normalized;
        rb.AddForce(dir * shootForce);
        
    }
}
