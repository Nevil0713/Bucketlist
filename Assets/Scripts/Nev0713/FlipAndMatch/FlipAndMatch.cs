using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlipAndMatch : MonoBehaviour
{
    [SerializeField] private FlipAndMatchObject[] cards;
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private int targetScore;

    FlipAndMatchObject m_firstCard, m_secondCard;
    private int m_currentScore;
    private int m_flipCount;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        StartRound();
    }

    public void StartRound()
    {
        m_currentScore++;
        m_flipCount = 0;
        
        List<int> numbers = Enumerable.Range(1, 8).ToList();
        List<int> randomNumbers = numbers.Concat(numbers).ToList();

        randomNumbers = randomNumbers.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].FlipCard();
            cards[i].SetCardSprite(cardSprites[randomNumbers[i]]);
            cards[i].SetCardNumber(randomNumbers[i]);
        }

        if(m_currentScore >= targetScore)
        {
            GameClear();
        }
    }

    public void FlipCheck(FlipAndMatchObject pFlipAndMatchObject)
    {
        if(m_flipCount == 0)
            m_firstCard = pFlipAndMatchObject;
        else
            m_flipCount++;

        if (m_flipCount == 2)
        {
            m_secondCard = pFlipAndMatchObject;

            if (m_firstCard.GetCardNumber() == m_secondCard.GetCardNumber())
            {
                Debug.Log("Correct");
            }
            else
            {
                Debug.Log("Wrong");
            }

            StartRound();
        }
    }

    private void GameClear()
    {
        // 다음 스토리 진행
    }
}
