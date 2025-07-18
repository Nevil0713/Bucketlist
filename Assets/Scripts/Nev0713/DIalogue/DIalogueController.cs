using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private string dialogueName;
    [SerializeField] private string nextSceneName;
    [SerializeField] private DialogueView dialogueView;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private bool startDialogueWhenStart = false;

    private IDialogueLoader m_dialogueLoader = new JsonDialogueLoader();
    private TitleButton m_sceneLoader = new TitleButton();

    private SceneData m_currentScene;
    private int m_dialogueIndex;
    private bool m_isTyping;
    private Coroutine m_typingCoroutine;
    private string m_previousBackground;

    private const float m_textSpeed = 0.04f;

    private void Start()
    {
        gameData.LastSceneName = SceneManager.GetActiveScene().name;
        dialogueView.HideAllUI();
        inputHandler.OnClick += OnScreenClicked;

        if (startDialogueWhenStart)
            StartFirstDialogue();
    }

    public GameData GetData()
    {
        return gameData;
    }

    public void MinigameFailed()
    {
        gameData.MinigameFails++;
        dialogueName += "Fail";
    }

    public void SetDialogueName(string pDialogueName)
    {
        dialogueName = pDialogueName;
    }

    public void StartFirstDialogue()
    {
        StartDialogue("Dialogues/" + dialogueName);
        
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
            ChangeScene(nextSceneName);
            return;
        }

        DialogueLine line = m_currentScene.dialogues[m_dialogueIndex];

        string backgroundPath = string.IsNullOrEmpty(line.background) ? null : "Backgrounds/" + line.background;
        string characterPath = string.IsNullOrEmpty(line.characterSprite) ? null : "Characters/" + line.character + "/" + line.characterSprite;
        string textboxPath = string.IsNullOrEmpty(line.textbox) ? null : "UI/Textbox/Textbox_" + line.textbox;
        string nameboxPath = string.IsNullOrEmpty(line.namebox) ? null : "UI/Namebox/Namebox_" + line.namebox;

        bool isBackgroundChanged = backgroundPath != m_previousBackground;

        void ShowUI()
        {
            dialogueView.SetBackground(LoadSprite(backgroundPath));
            dialogueView.SetCharacterSprite(LoadSprite(characterPath));
            dialogueView.SetDialogueTextbox(LoadSprite(textboxPath));
            dialogueView.SetCharacterNamebox(LoadSprite(nameboxPath));
        }

        if (isBackgroundChanged)
        {
            ScreenFader.FadeIn(() =>
            {
                ShowUI();
                dialogueView.HideUI();
                ScreenFader.FadeOut(() =>
                {
                    dialogueView.ShowUI();
                    PlayTyping(line);
                });
            });
        }
        else
        {
            ShowUI();
            dialogueView.ShowUI();
            PlayTyping(line);
        }

        m_previousBackground = backgroundPath;
    }

    private void PlayTyping(DialogueLine line)
    {
        if (m_typingCoroutine != null) StopCoroutine(m_typingCoroutine);
        m_typingCoroutine = StartCoroutine(TypeSentence(line.text, () =>
        {
            if (line.choices != null && line.choices.Count > 0)
                StartCoroutine(ShowChoicesWithDelay(line));
        }));
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

    private IEnumerator ShowChoicesWithDelay(DialogueLine line)
    {
        yield return new WaitForSeconds(1f);
        dialogueView.ShowChoices(line.choices, OnChoiceSelected);
    }

    private void OnScreenClicked()
    {
        if (ScreenFader.IsFading || m_currentScene == null)
            return;

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

        if (string.IsNullOrEmpty(pNextScene))
        {
            m_dialogueIndex++;
            ShowCurrentDialogue();
        }
        else
        {
            StartDialogue("Dialogues/" + pNextScene);
        }
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
        if (string.IsNullOrEmpty(pPath))
            return null;
        
        return Resources.Load<Sprite>(pPath);
    }
}
