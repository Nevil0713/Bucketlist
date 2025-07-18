using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public DialogueController dialogueController;
    public GameObject fish;
    public Transform[] spawnPoints;
    public float spawnInterval = 1.5f;
    public float moleStayTime = 1f;

    public float gameDuration = 30f; // ���� ���� �ð� (��)
    public TextMeshProUGUI timerText; // ���� �ð��� ǥ���� UI

    public bool gameCleared = false;

    private float remainingTime;

    void Start()
    {
        remainingTime = gameDuration;

        // �δ��� ���� �ݺ�
        StartCoroutine(SpawnFish());

        // Ÿ�̸� ����
        StartCoroutine(GameTimer());
    }

    IEnumerator SpawnFish()
    {
        while (true)
        {
            int index = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[index];
            fish.transform.position = spawnPoint.position;
            fish.SetActive(true);
            yield return new WaitForSeconds(2);
            fish.SetActive(false);
        }
    }

    IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Ÿ�̸� ����
        timerText.text = "Time: 0";
        CancelInvoke(nameof(SpawnFish));
        Debug.Log("���� ����!");

        if (!gameCleared)
            dialogueController.MinigameFailed();


        gameObject.SetActive(false);
        dialogueController.StartFirstDialogue();
    }
}
