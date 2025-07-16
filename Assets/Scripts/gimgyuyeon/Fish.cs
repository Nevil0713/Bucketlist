using UnityEngine;

public class Fish : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("fish clicked!");
        ScoreManager.Instance.AddScore(1);
        Destroy(gameObject);
    }
}
