using UnityEngine;

public class ShooterController : MonoBehaviour
{
    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // ���Ϳ� ���콺 ������ ���� ���
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ȸ�� ���� (Z�ุ ȸ��)
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // -90���� ���밡 ���� ���ϰ� ����
    }
}
