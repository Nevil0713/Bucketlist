using UnityEngine;

public class mole : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Mole clicked!");
        ScoreManager.Instance.AddScore(1);
        Destroy(gameObject); // Ŭ���� �δ��� ����
    }
}
