using System.Collections;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Header("필수 컴포넌트 연결")]
    public DialogueView m_dialogueView;
    public InputHandler m_inputHandler;

    private IDialogueLoader m_dialogueLoader = new JsonDialogueLoader();
    private SceneLoader m_sceneLoader = new SceneLoader();

    private SceneData m_currentScene;
    private int m_dialogueIndex;
    private bool m_isTyping;
    private Coroutine m_typingCoroutine;

    private const float m_textSpeed = 0.04f;

    private void Start()
    {
        m_inputHandler.OnClick += OnScreenClicked;
        StartDialogue("Dialogues/Intro");
    }

    public void StartDialogue(string pFileName)
    {
        m_currentScene = m_dialogueLoader.LoadDialogue(pFileName);
        m_dialogueIndex = 0;
        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        if (m_currentScene == null) return;

        if (m_dialogueIndex >= m_currentScene.dialogues.Count)
        {
            ChangeScene("NextScene");
            return;
        }

        DialogueLine line = m_currentScene.dialogues[m_dialogueIndex];

        ScreenFader.FadeIn(() =>
        {
            m_dialogueView.SetCharacterName(line.character);
            
            string backgroundPath = string.IsNullOrEmpty(line.background) ? null : "Backgrounds/" + line.background;
            string characterPath = string.IsNullOrEmpty(line.characterSprite) ? null : "Characters/" + line.characterSprite;

            m_dialogueView.SetBackground(LoadSprite(backgroundPath));
            m_dialogueView.SetCharacterSprite(LoadSprite(characterPath));

            ScreenFader.FadeOut(() =>
            {
                if (m_typingCoroutine != null) StopCoroutine(m_typingCoroutine);
                m_typingCoroutine = StartCoroutine(TypeSentence(line.text, () =>
                {
                    if (line.choices != null && line.choices.Count > 0)
                    {
                        m_dialogueView.ShowChoices(line.choices, OnChoiceSelected);
                    }
                }));
            });
        });
    }

    private IEnumerator TypeSentence(string pSentence, System.Action pOnComplete)
    {
        m_isTyping = true;
        m_dialogueView.SetDialogueText("");

        foreach (char letter in pSentence)
        {
            m_dialogueView.SetDialogueText(m_dialogueView.dialogueText.text + letter);
            yield return new WaitForSeconds(m_textSpeed);
        }

        m_isTyping = false;
        pOnComplete?.Invoke();
    }

    private void OnScreenClicked()
    {
        if (ScreenFader.IsFading) return;
        if (m_currentScene == null) return;

        DialogueLine line = m_currentScene.dialogues[m_dialogueIndex];

        if (line.choices != null && line.choices.Count > 0)
            return;

        if (m_isTyping)
        {
            if (m_typingCoroutine != null) StopCoroutine(m_typingCoroutine);
            m_dialogueView.SetDialogueText(line.text);
            m_isTyping = false;
        }
        else
        {
            m_dialogueIndex++;
            ShowCurrentDialogue();
        }
    }

    private void OnChoiceSelected(string pNextScene)
    {
        m_dialogueView.HideChoices();
        StartDialogue("Dialogues/" + pNextScene);
    }

    private void ChangeScene(string pSceneName)
    {
        ScreenFader.FadeIn(() =>
        {
            m_sceneLoader.LoadScene(pSceneName, () =>
            {
                ScreenFader.FadeOut(null);
            });
        });
    }

    private Sprite LoadSprite(string pName)
    {
        if (string.IsNullOrEmpty(pName)) return null;
        Sprite sprite = Resources.Load<Sprite>(pName);
        if (sprite == null)
        {
            Debug.LogWarning($"스프라이트 로드 실패: {pName}");
        }
        return sprite;
    }
}
