using UnityEngine;
using UnityEngine.UI;


public class SequenceMemoryObject : MonoBehaviour
{
    private int buttonNumber;
    private SequenceMemoryManager m_manager;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_manager = GetComponentInParent<SequenceMemoryManager>();
        GetComponent<Button>().onClick.AddListener(OnClick);
        GetComponent<Image>().color = Color.white;
    }

    public void SetButtonNumber(int pNumber)
    {
        buttonNumber = pNumber;
    }

    private void OnClick()
    {
        Debug.Log("Clicked");
        GetComponent<Image>().color = Color.black;
        GetComponent<Button>().interactable = false;
        StartCoroutine(m_manager.OnButtonClicked(buttonNumber));
    }
}
