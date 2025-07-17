using UnityEngine;
using UnityEngine.UI;

public class PlayerFaceController : MonoBehaviour
{
    [SerializeField] private Sprite[] faces;
    private Image playerFace;

    private void Awake()
    {
        playerFace = GetComponent<Image>();
    }

    public void SetPlayerFace(int pFaceNumber)
    {
        playerFace.sprite = faces[pFaceNumber];
    }

    public void ResetPlayerFace()
    {
        playerFace.sprite = faces[0];
    }
}
