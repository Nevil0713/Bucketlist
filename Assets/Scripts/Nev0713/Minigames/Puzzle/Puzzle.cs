using UnityEngine;


public class Puzzle : MonoBehaviour
{
    [SerializeField] private PuzzleObject[] puzzles;
    [SerializeField] private Transform[] puzzlePositions;

    private bool[] m_correctPuzzles;
    private bool m_isCleared;

    private void Awake()
    {
        m_correctPuzzles = new bool[puzzles.Length];
        m_isCleared = false;

        for (int i  = 0; i < m_correctPuzzles.Length; i++)
        {
            m_correctPuzzles[i] = false;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            for(int i = 0; i < puzzles.Length; i++)
            {
                float distance = Vector3.Distance(puzzles[i].transform.position, puzzlePositions[i].position);
                if (distance <= 0.25f)
                {
                    puzzles[i].transform.position = puzzlePositions[i].transform.position;
                    m_correctPuzzles[i] = true;
                }
                else
                {
                    m_correctPuzzles[i] = false;
                }
            }
        }

        for(int i = 0, count = 0; i < m_correctPuzzles.Length; i++)
        {
            if (m_correctPuzzles[i])
                count++;
            else
                break;

            if(count == puzzles.Length)
                m_isCleared = true;
        }
    }

    public bool IsCleared()
    {
        return m_isCleared;
    }
}
