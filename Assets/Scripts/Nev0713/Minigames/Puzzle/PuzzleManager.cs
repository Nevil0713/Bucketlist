using UnityEngine;


public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private DialogueController dialogueManager;
    [SerializeField] private GameObject puzzle;

    bool isCleared = false;

    private void Awake()
    {
        puzzle.SetActive(false);
        isCleared = false;
        ScreenFader.FadeOut();
        Invoke("Init", 1);
    }

    private void Update()
    {
        if (puzzle.GetComponent<Puzzle>().IsCleared() && !isCleared)
        {
            GameClear();
        }
    }

    public void Init()
    {
        PuzzleStart();
    }

    private void PuzzleStart()
    {
        puzzle.SetActive(true);
    }

    private void GameClear()
    {
        Debug.Log("Clear");
        isCleared = true;
        puzzle.SetActive(false);
        dialogueManager.StartFirstDialogue();
    }
}
