using UnityEngine;

public class Fish : MonoBehaviour
{
    public void OnClicked()
    {
        Debug.Log("fish clicked!");
        ScoreManager.Instance.AddScore(1);
        gameObject.SetActive(false);
    }
}
