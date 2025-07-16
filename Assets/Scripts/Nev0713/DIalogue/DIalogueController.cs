using System.Collections;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueView dialogueView;
    [SerializeField] private InputHandler inputHandler;

    private IDialogueLoader m_dialogueLoader = new JsonDialogueLoader();
    private SceneLoader m_sceneLoader = new SceneLoader();

    private SceneData m_currentScene;
    private int m_dialogueIndex;
    private bool m_isTyping;
    private Coroutine m_typingCoroutine;
    private string m_previousBackground;

    private const float m_textSpeed = 0.04f;

    private void Start()
    {
        dialogueView.HideAllUI();
        inputHandler.OnClick += OnScreenClicked;
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
        if (m_currentScene == null)
            return;

        if (m_dialogueIndex >= m_currentScene.dialogues.Count)
        {
            ChangeScene("NextScene");
            return;
        }

        DialogueLine line = m_currentScene.dialogues[m_dialogueIndex];

        string backgroundPath = string.IsNullOrEmpty(line.background) ? null : "Backgrounds/" + line.background;
        string characterPath = string.IsNullOrEmpty(line.characterSprite) ? null : "Characters/" + line.characterSprite;

        bool isBackgroundChanged = backgroundPath != m_previousBackground;

        void ShowUI()
        {
            dialogueView.SetCharacterName(line.character);
            dialogueView.SetBackground(LoadSprite(backgroundPath));
            dialogueView.SetCharacterSprite(LoadSprite(characterPath));
        }

        if (isBackgroundChanged)
        {
            ScreenFader.FadeIn(() =>
            {
                ShowUI();

                dialogueView.HideUIForTransition();

                ScreenFader.FadeOut(() =>
                {
                    dialogueView.ShowUI();

                    if (m_typingCoroutine != null) StopCoroutine(m_typingCoroutine);
                    m_typingCoroutine = StartCoroutine(TypeSentence(line.text, () =>
                    {
                        if (line.choices != null && line.choices.Count > 0)
                            dialogueView.ShowChoices(line.choices, OnChoiceSelected);
                    }));
                });
            });
        }
        else
        {
            ShowUI();
            dialogueView.ShowUI();

            if (m_typingCoroutine != null) StopCoroutine(m_typingCoroutine);
            m_typingCoroutine = StartCoroutine(TypeSentence(line.text, () =>
            {
                if (line.choices != null && line.choices.Count > 0)
                    dialogueView.ShowChoices(line.choices, OnChoiceSelected);
            }));
        }

        m_previousBackground = backgroundPath;
    }


    private IEnumerator TypeSentence(string pSentence, System.Action pOnComplete)
    {
        m_isTyping = true;
        dialogueView.SetDialogueText("");

        foreach (char letter in pSentence)
        {
            dialogueView.AppendDialogueLetter(letter);
            yield return new WaitForSeconds(m_textSpeed);
        }

        m_isTyping = false;
        pOnComplete?.Invoke();
    }

    private void OnScreenClicked()
    {
        if (ScreenFader.IsFading || m_currentScene == null) return;

        DialogueLine line = m_currentScene.dialogues[m_dialogueIndex];

        if (line.choices != null && line.choices.Count > 0)
            return;

        if (m_isTyping)
        {
            if (m_typingCoroutine != null) StopCoroutine(m_typingCoroutine);
            dialogueView.SetDialogueText(line.text);
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
        dialogueView.HideChoices();
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

    private Sprite LoadSprite(string pPath)
    {
        if (string.IsNullOrEmpty(pPath)) return null;
        return Resources.Load<Sprite>(pPath);
    }
}
