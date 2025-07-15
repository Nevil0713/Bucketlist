using UnityEngine;
using UnityEngine.UI;

public class FlipAndMatchObject : MonoBehaviour
{
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    private Image m_image;
    private FlipAndMatchManager m_manager;

    private bool m_isFlipped = false;
    private bool m_isMatched = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_image = GetComponent<Image>();
        m_image.sprite = backSprite;
        m_manager = FindAnyObjectByType<FlipAndMatchManager>();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (m_isFlipped || m_isMatched || m_manager.IsBusy)
            return;

        FlipFront();
        m_manager.OnCardClicked(this);
    }

    public void SetMatched()
    {
        m_isMatched = true;
        GetComponent<Button>().interactable = false;
    }

    public void FlipFront()
    {
        gameObject.GetComponent<Image>().sprite = frontSprite;
    }

    public void FlipBack()
    {
        gameObject.GetComponent<Image>().sprite = backSprite;
    }

    public void SetFrontSprite(Sprite pSprite)
    {
        frontSprite = pSprite;
    }
    public Sprite GetFrontSprite()
    {
        return frontSprite;
    }
}
