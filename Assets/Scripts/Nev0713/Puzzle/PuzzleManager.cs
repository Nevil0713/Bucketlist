using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    private Puzzle m_puzzle;
    private int m_puzzleNumber = 0;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (m_puzzle.IsCleared() && m_puzzleNumber < puzzles.Length)
        {
            m_puzzle.gameObject.SetActive(false);
            PuzzleClear();
        }
    }

    public void Init()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].SetActive(false);
        }
        PuzzleStart();
    }

    private void PuzzleStart()
    {
        puzzles[m_puzzleNumber].SetActive(true);
        m_puzzle = puzzles[m_puzzleNumber].GetComponent<Puzzle>();
    }

    private void PuzzleClear()
    {
        m_puzzleNumber++;
        if(m_puzzleNumber == puzzles.Length)
        {
            GameClear();
            return;
        }
        PuzzleStart();
        
    }

    private void GameClear()
    {
        // 다음 스토리 진행
    }
}
