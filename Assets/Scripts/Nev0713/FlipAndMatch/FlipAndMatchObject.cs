using UnityEngine;
using UnityEngine.UI;

public class FlipAndMatchObject : MonoBehaviour
{
    [SerializeField] private FlipAndMatch m_flipAndMatch;
    [SerializeField] private Sprite[] m_cardSprites;
    private int m_spriteNumber;
    private int m_cardNumber;

    public void FlipCard()
    {
        gameObject.GetComponent<Image>().sprite = m_cardSprites[m_spriteNumber++];
        m_spriteNumber %= 2;
        m_flipAndMatch.FlipCheck(this);
    }

    public void SetCardSprite(Sprite pSprite)
    {
        m_cardSprites[1] = pSprite;
    }

    public void SetCardNumber(int pNumber)
    {
        m_cardNumber = pNumber;
    }

    public int GetCardNumber()
    {
        return m_cardNumber;
    }
}
