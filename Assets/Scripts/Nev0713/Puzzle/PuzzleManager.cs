using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    private Puzzle m_puzzle;
    private int m_puzzleNumber = 0;

    private void Awake()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].SetActive(false);
        }
        PuzzleStart();
    }

    private void Update()
    {
        if (m_puzzle.IsCleared())
        {
            m_puzzle.gameObject.SetActive(false);
            PuzzleClear();
        }
    }

    public void PuzzleStart()
    {
        puzzles[m_puzzleNumber].SetActive(true);
        m_puzzle = puzzles[m_puzzleNumber].GetComponent<Puzzle>();
    }

    public void PuzzleClear()
    {
        m_puzzleNumber++;
        if(m_puzzleNumber == puzzles.Length)
        {
            GameClear();
        }
        else
        {
            PuzzleStart();
        }
    }

    public void GameClear()
    {
        SceneManager.LoadScene("");
    }
}
