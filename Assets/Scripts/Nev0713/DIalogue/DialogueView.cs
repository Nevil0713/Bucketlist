using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUIContainer;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image dialogueTextbox;
    [SerializeField] private Image characterNamebox;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choiceButtonPrefab;

    private Action<string> m_onChoiceSelected;

    public void ShowUI()
    {
        dialogueUIContainer.SetActive(true);
    }

    public void HideUI()
    {
        dialogueUIContainer.SetActive(false);
    }

    public void HideAllUI()
    {
        dialogueUIContainer.SetActive(false);
        choicePanel.SetActive(false);
    }

    public void SetDialogueTextbox(Sprite pSprite)
    {
        if (pSprite != null)
        {
            dialogueTextbox.sprite = pSprite;
            dialogueTextbox.gameObject.SetActive(true);
        }
        else
        {
            dialogueTextbox.gameObject.SetActive(false);
        }
    }

    public void SetCharacterNamebox(Sprite pSprite)
    {
        if (pSprite != null)
        {
            characterNamebox.sprite = pSprite;
            characterNamebox.gameObject.SetActive(true);
        }
        else
        {
            characterNamebox.gameObject.SetActive(false);
        }
    }

    public void SetDialogueText(string pText)
    {
        dialogueText.text = pText;
    }

    public void AppendDialogueLetter(char pChar)
    {
        dialogueText.text += pChar;
    }

    public void SetBackground(Sprite pSprite)
    {
        if (pSprite != null)
            backgroundImage.sprite = pSprite;
    }

    public void SetCharacterSprite(Sprite pSprite)
    {
        if (pSprite != null)
        {
            characterImage.sprite = pSprite;
            characterImage.gameObject.SetActive(true);

            float aspectRatio = pSprite.bounds.size.x / pSprite.bounds.size.y;
            float targetHeight = 8f;
            float targetWidth = targetHeight * aspectRatio;

            RectTransform rect = characterImage.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(targetWidth, targetHeight);
            rect.localScale = Vector3.one;
        }
        else
        {
            characterImage.gameObject.SetActive(false);
        }
    }

    public void ShowChoices(List<Choice> pChoices, Action<string> pOnChoiceSelected)
    {
        m_onChoiceSelected = pOnChoiceSelected;
        choicePanel.SetActive(true);

        foreach (Transform child in choicePanel.transform)
            Destroy(child.gameObject);

        foreach (var choice in pChoices)
        {
            var localChoice = choice;
            Button button = Instantiate(choiceButtonPrefab, choicePanel.transform);
            button.GetComponentInChildren<Text>().text = localChoice.text;

            button.onClick.AddListener(() =>
            {
                m_onChoiceSelected?.Invoke(localChoice.nextScene);
            });
        }
    }

    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }
}
