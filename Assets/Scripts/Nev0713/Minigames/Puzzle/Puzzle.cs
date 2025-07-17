using UnityEngine;


public class Puzzle : MonoBehaviour
{
    [SerializeField] private GameObject puzzle;
    [SerializeField] private GameObject puzzlePosition;

    private PuzzleObject[] puzzlePieces;
    private Transform[] puzzlePositions;
    private bool[] m_correctPuzzles;
    private bool m_isCleared;

    private void Awake()
    {
        puzzlePieces = puzzle.GetComponentsInChildren<PuzzleObject>();
        puzzlePositions = puzzlePosition.GetComponentsInChildren<Transform>();
        
        for(int i = 1; i < puzzlePositions.Length; i++)
        {
            puzzlePositions[i - 1] = puzzlePositions[i];
        }

        m_correctPuzzles = new bool[puzzlePieces.Length];
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
            for(int i = 0; i < puzzlePieces.Length; i++)
            {
                float distance = Vector3.Distance(puzzlePieces[i].transform.position, puzzlePositions[i].position);
                if (distance <= 0.25f)
                {
                    puzzlePieces[i].transform.position = puzzlePositions[i].transform.position;
                    puzzlePieces[i].GetComponent<BoxCollider2D>().enabled = false;
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

            if(count == puzzlePieces.Length)
                m_isCleared = true;
        }
    }

    public bool IsCleared()
    {
        return m_isCleared;
    }
}
