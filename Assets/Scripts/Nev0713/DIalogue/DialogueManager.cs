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
            Debug.Log("��ȭ ����");
            return;
        }

        DialogueLine line = currentScene.dialogues[dialogueIndex];

        characterNameText.text = line.character;
        dialogueText.text = line.text;

        // �������� �ִ� ���
        if (line.choices != null && line.choices.Count > 0)
        {
            ShowChoices(line.choices);
        }
        else
        {
            // ���� ���� �ѱ�� (��: Ŭ������ ����)
            Invoke(nameof(WaitForNext), 2f); // �ڵ����� ���� ��� (����)
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

        // ���� ��ư ����
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

    // ��縦 Ŭ������ �ѱ�� ���� ���:
    public void OnDialogueClick()
    {
        if (currentScene.dialogues[dialogueIndex].choices == null)
        {
            dialogueIndex++;
            ShowCurrentDialogue();
        }
    }
}
