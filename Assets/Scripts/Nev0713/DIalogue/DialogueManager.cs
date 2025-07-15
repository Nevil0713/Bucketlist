using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Choice
{
    public string text;
    public string nextScene;
}

[Serializable]
public class DialogueLine
{
    public string character;
    public string text;
    public string expression;
    public List<Choice> choices;
}

[Serializable]
public class SceneData
{
    public string sceneName;
    public string background;
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

    private SceneData currentScene;
    private int dialogueIndex = 0;

    void Start()
    {
        LoadDialogue("Dialogues/Test"); // Resources/Dialogs/Test.json
        ShowCurrentDialogue();
    }

    void LoadDialogue(string fileName)
    {
        TextAsset json = Resources.Load<TextAsset>(fileName);
        currentScene = JsonUtility.FromJson<SceneData>(json.text);
        dialogueIndex = 0;

        backgroundImage.sprite = Resources.Load<Sprite>($"{backgroundFolder}/{currentScene.background}");
    }

    void ShowCurrentDialogue()
    {
        if (dialogueIndex >= currentScene.dialogues.Count)
        {
            Debug.Log("대화 종료");
            return;
        }

        DialogueLine line = currentScene.dialogues[dialogueIndex];

        characterNameText.text = line.character;
        dialogueText.text = line.text;

        // 선택지가 있는 경우
        if (line.choices != null && line.choices.Count > 0)
        {
            ShowChoices(line.choices);
        }
        else
        {
            // 다음 대사로 넘기기 (예: 클릭으로 진행)
            Invoke(nameof(WaitForNext), 2f); // 자동으로 다음 대사 (예시)
        }
    }

    void WaitForNext()
    {
        dialogueIndex++;
        ShowCurrentDialogue();
    }

    void ShowChoices(List<Choice> choices)
    {
        choicePanel.SetActive(true);

        // 기존 버튼 제거
        foreach (Transform child in choicePanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Choice choice in choices)
        {
            Button button = Instantiate(choiceButtonPrefab, choicePanel.transform);
            button.GetComponentInChildren<Text>().text = choice.text;

            button.onClick.AddListener(() =>
            {
                choicePanel.SetActive(false);
                LoadDialogue($"Dialogues/{choice.nextScene}");
                ShowCurrentDialogue();
            });
        }
    }

    // 대사를 클릭으로 넘기고 싶을 경우:
    public void OnDialogueClick()
    {
        if (currentScene.dialogues[dialogueIndex].choices == null)
        {
            dialogueIndex++;
            ShowCurrentDialogue();
        }
    }
}
