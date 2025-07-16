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

            // 원본 크기 구하기
            float originalWidth = pSprite.bounds.size.x;
            float originalHeight = pSprite.bounds.size.y;
            float aspectRatio = originalWidth / originalHeight;

            // 기준 높이 설정
            float targetHeight = 8;
            float targetWidth = targetHeight * aspectRatio;

            // RectTransform 사이즈 조정
            RectTransform rect = characterImage.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(targetWidth, targetHeight);

            // 비율 확대
            float scaleMultiplier = 1.0f;
            rect.localScale = Vector3.one * scaleMultiplier;
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
