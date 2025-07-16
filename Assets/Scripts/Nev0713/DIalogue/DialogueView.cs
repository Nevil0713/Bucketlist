using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUIContainer;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private Text characterNameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choiceButtonPrefab;

    private Action<string> m_onChoiceSelected;

    public void HideAllUI()
    {
        dialogueUIContainer.SetActive(false);
        choicePanel.SetActive(false);
    }

    public void ShowUI()
    {
        dialogueUIContainer.SetActive(true);
    }

    public void HideUIForTransition()
    {
        dialogueUIContainer.SetActive(false);
    }

    public void SetCharacterName(string pName)
    {
        characterNameText.text = pName ?? "";
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
            Button btn = Instantiate(choiceButtonPrefab, choicePanel.transform);
            btn.GetComponentInChildren<Text>().text = localChoice.text;

            btn.onClick.AddListener(() =>
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
