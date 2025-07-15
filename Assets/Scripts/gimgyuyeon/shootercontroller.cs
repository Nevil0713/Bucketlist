using UnityEngine;

public class ShooterController : MonoBehaviour
{
    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // 슈터와 마우스 사이의 방향 계산
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 회전 적용 (Z축만 회전)
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // -90도는 막대가 위를 향하게 조정
    }
}
