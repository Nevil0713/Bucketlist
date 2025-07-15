using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

[Serializable]
public class Choice
{
    public string text;
    public string nextDIalogue;
}

[Serializable]
public class DialogueLine
{
    public string character;
    public string text;
    public string expression;
    public string background;
    public string scene;
    public List<Choice> choices;
}

[Serializable]
public class DialogueData
{
    public string dialogueName;
    public List<DialogueLine> dialogues;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text characterNameText;
    [SerializeField] private Text dialogueText;

    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choiceButtonPrefab;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private string backgroundFolder = "Backgrounds";

    private DialogueData currentDIalogue;
    private string currentBackground;
    private int dialogueIndex = 0;

    private void Awake()
    {
        LoadDialogue("Dialogues/Intro");
        StartCoroutine(ShowCurrentDialogue());
    }

    private void LoadDialogue(string pFileName)
    {
        TextAsset json = Resources.Load<TextAsset>(pFileName);
        currentDIalogue = JsonUtility.FromJson<DialogueData>(json.text);
        dialogueIndex = 0;

    }

    private IEnumerator ShowCurrentDialogue()
    {
        if (dialogueIndex >= currentDIalogue.dialogues.Count)
        {
            Debug.Log("대화 종료");
            yield break;
        }

        DialogueLine line = currentDIalogue.dialogues[dialogueIndex];

        characterNameText.text = line.character;
        dialogueText.text = line.text;

        if(line.background != currentBackground)
        {
            currentBackground = line.background;
            //페이드인
            backgroundImage.sprite = Resources.Load<Sprite>($"{backgroundFolder}/{line.background}");
            //페이드아웃
        }

        if (line.choices != null && line.choices.Count > 0)
        {
            ShowChoices(line.choices);
        }
        else
        {
            // 다음 대사로 넘기기 (예: 클릭으로 진행)
            dialogueIndex++;
            yield return new WaitForSeconds(1);
            StartCoroutine(ShowCurrentDialogue());
        }
    }

    void ShowChoices(List<Choice> pChoices)
    {
        choicePanel.SetActive(true);

        foreach (Transform child in choicePanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Choice choice in pChoices)
        {
            Button button = Instantiate(choiceButtonPrefab, choicePanel.transform);
            button.GetComponentInChildren<Text>().text = choice.text;

            button.onClick.AddListener(() =>
            {
                choicePanel.SetActive(false);
                LoadDialogue($"Dialogues/{choice.nextDIalogue}");
                StartCoroutine(ShowCurrentDialogue());
            });
        }
    }

    // 대사를 클릭으로 넘기고 싶을 경우:
    public void OnDialogueClick()
    {
        if (currentDIalogue.dialogues[dialogueIndex].choices == null)
        {
            dialogueIndex++;
            StartCoroutine(ShowCurrentDialogue());
        }
    }
}
