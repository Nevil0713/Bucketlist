using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlipAndMatchManager : MonoBehaviour
{
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private FlipAndMatchObject[] cards;
    [SerializeField] private int targetScore;

    private FlipAndMatchObject m_firstCard;
    private FlipAndMatchObject m_secondCard;
    private int m_currentScore;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        cards = GetComponentsInChildren<FlipAndMatchObject>();
        List<Sprite> sprites = GetShuffledSpritePairs();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetFrontSprite(sprites[i]);
        }
        m_currentScore = 0;
    }

    public void OnCardClicked(FlipAndMatchObject pCard)
    {
        if (IsBusy) return;

        if (m_firstCard == null)
        {
            m_firstCard = pCard;
        }
        else if (m_secondCard == null && pCard != m_firstCard)
        {
            m_secondCard = pCard;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        IsBusy = true;
        yield return new WaitForSeconds(1f);

        if (m_firstCard.GetFrontSprite() == m_secondCard.GetFrontSprite())
        {
            Debug.Log("correct");
            m_currentScore += 30;
            m_firstCard.SetMatched();
            m_secondCard.SetMatched();

            if(m_currentScore >= targetScore)
            {
                GameClear();
            }
        }
        else
        {
            Debug.Log("wrong");
            m_currentScore -= 10;

            if(m_currentScore < 0)
                m_currentScore = 0;

            m_firstCard.FlipBack();
            m_secondCard.FlipBack();
        }

        m_firstCard = null;
        m_secondCard = null;
        IsBusy = false;
    }

    private List<Sprite> GetShuffledSpritePairs()
    {
        List<Sprite> spritePairs = new List<Sprite>();

        foreach (var sprite in frontSprites)
        {
            spritePairs.Add(sprite);
            spritePairs.Add(sprite);
        }

        for (int i = 0; i < spritePairs.Count; i++)
        {
            int rand = Random.Range(i, spritePairs.Count);
            (spritePairs[i], spritePairs[rand]) = (spritePairs[rand], spritePairs[i]);
        }

        return spritePairs;
    }

    private void GameClear()
    {
        Debug.Log("Clear");
        // 다음 스토리 진행
    }
}
