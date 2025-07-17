using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlipAndMatchManager : MonoBehaviour
{
    [SerializeField] private DialogueController dialogueManager;
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private FlipAndMatchObject[] cards;
    [SerializeField] private int targetScore;
    [SerializeField] private int tryCount;
    [SerializeField] private Image tryCountImage;
    [SerializeField] private Text tryCountText;

    private FlipAndMatchObject m_firstCard;
    private FlipAndMatchObject m_secondCard;
    private int m_currentScore;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        cards = GetComponentsInChildren<FlipAndMatchObject>();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(false);
        }
        tryCountImage.gameObject.SetActive(false);
        tryCountText.gameObject.SetActive(false);
        ScreenFader.FadeOut();
        Invoke("Init", 1);
    }

    public void Init()
    {
        List<Sprite> sprites = GetShuffledSpritePairs();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(true);
            cards[i].SetFrontSprite(sprites[i]);
        }
        tryCountImage.gameObject.SetActive(true);
        tryCountText.gameObject.SetActive(true);
        tryCountText.text = tryCount.ToString();
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
            m_currentScore++;
            tryCount++;
            if (tryCount >= 5)
                tryCount = 5;

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
            tryCount--;

            if (tryCount <= 0)
            {
                GameOver();
                yield break;
            }

            m_firstCard.FlipBack();
            m_secondCard.FlipBack();
        }

        m_firstCard = null;
        m_secondCard = null;
        IsBusy = false;
        tryCountText.text = tryCount.ToString();
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
        dialogueManager.StartFirstDialogue();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(false);
        }
        tryCountImage.gameObject.SetActive(false);
        tryCountText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void GameOver()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(false);
        }
        tryCountImage.gameObject.SetActive(false);
        tryCountText.gameObject.SetActive(false);
    }
}
