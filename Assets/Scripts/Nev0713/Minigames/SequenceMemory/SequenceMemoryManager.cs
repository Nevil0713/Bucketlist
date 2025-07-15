using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceMemoryManager : MonoBehaviour
{
    private SequenceMemoryObject[] m_buttons;
    private List<int> m_sequence;
    private int m_targetRound;
    private int m_currentRound;
    private int m_targetStep;
    private int m_currentStep;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_buttons = GetComponentsInChildren<SequenceMemoryObject>();
        for (int i = 0; i < m_buttons.Length; i++)
        {
            m_buttons[i].SetButtonNumber(i);
        }

        m_targetRound = 5;
        m_currentRound = 1;
        m_targetStep = m_currentRound;
        m_currentStep = 0;

        StartCoroutine(StartRound());
    }

    public IEnumerator StartRound()
    {
        for (int i = 0; i < m_buttons.Length; i++)
        {
            SetButton(i, false, Color.white);
        }

        m_sequence = GetRandomNumbers(m_targetStep, m_buttons.Length);
        for (int i = 0; i < m_sequence.Count; i++)
        {
            yield return new WaitForSeconds(1);
            ShowSequence(m_sequence[i]);
            yield return new WaitForSeconds(1);
            HideSequence(m_sequence[i]);
        }
        yield return new WaitForSeconds(1);

        for (int i = 0; i < m_buttons.Length; i++)
        {
            m_buttons[i].GetComponent<Button>().interactable = true;
        }
    }

    private void ShowSequence(int pRandomNumber)
    {
        Debug.Log("ShowSequence");
        m_buttons[pRandomNumber].GetComponent<Image>().color = Color.black;
    }

    private void HideSequence(int pRandomNumber)
    {
        Debug.Log("HideSequence");
        m_buttons[pRandomNumber].GetComponent<Image>().color = Color.white;
    }

    private List<int> GetRandomNumbers(int pCount, int pMax)
    {
        List<int> source = new List<int>();
        for (int i = 0; i < pMax; i++)
            source.Add(i);

        List<int> result = new List<int>();
        for (int i = 0; i < pCount; i++)
        {
            int randIndex = Random.Range(0, source.Count);
            result.Add(source[randIndex]);
            source.RemoveAt(randIndex);
        }

        return result;
    }

    public IEnumerator OnButtonClicked(int pButtonNumber)
    {
        if (m_sequence[m_currentStep] == pButtonNumber)
        {
            Debug.Log("Correct");
            m_currentStep++;

            if (m_currentStep >= m_targetStep)
            {
                Debug.Log("NextRound / Round " + m_currentRound);
                m_currentRound++;
                m_targetStep = m_currentRound;
                m_currentStep = 0;

                if (m_currentRound > m_targetRound)
                {
                    for (int i = 0; i < m_buttons.Length; i++)
                    {
                        SetButton(i, false);
                    }

                    GameClear();
                    yield break;
                }

                for (int i = 0; i < m_buttons.Length; i++)
                {
                    SetButton(i, false, Color.green);
                }

                yield return new WaitForSeconds(1);

                for (int i = 0; i < m_buttons.Length; i++)
                {
                    SetButton(i, Color.white);
                }

                StartCoroutine(StartRound());
            }
        }
        else
        {
            Debug.Log("Wrong");
            m_currentRound = 1;
            m_targetStep = m_currentRound;
            m_currentStep = 0;

            for (int i = 0; i < m_buttons.Length; i++)
            {
                SetButton(i, false, Color.red);
            }

            yield return new WaitForSeconds(1);

            for (int i = 0; i < m_buttons.Length; i++)
            {
                SetButton(i, Color.white);
            }

            StartCoroutine(StartRound());
        }
    }

    private void SetButton(int pNumber, bool pValue, Color pColor)
    {
        m_buttons[pNumber].GetComponent<Button>().interactable = pValue;
        m_buttons[pNumber].GetComponent<Image>().color = pColor;
    }

    private void SetButton(int pNumber, Color pColor)
    {
        m_buttons[pNumber].GetComponent<Image>().color = pColor;
    }

    private void SetButton(int pNumber, bool pValue)
    {
        m_buttons[pNumber].GetComponent<Button>().interactable = pValue;
    }

    private void GameClear()
    {
        Debug.Log("Clear");
        // 다음 스토리 진행
    }
}
