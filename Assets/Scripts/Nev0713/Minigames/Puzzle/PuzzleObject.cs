using UnityEngine;


public class PuzzleObject : MonoBehaviour
{
    private Vector3 m_mouseWorldPos;
    private bool m_isClicked;

    private void Update()
    {
        m_mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_mouseWorldPos.z = 0f;

        if(m_isClicked)
        {
            transform.position = m_mouseWorldPos;
        }
    }

    private void OnMouseDown()
    {
        m_isClicked = true;
    }
    private void OnMouseUp()
    {
        m_isClicked = false;
    }
}
