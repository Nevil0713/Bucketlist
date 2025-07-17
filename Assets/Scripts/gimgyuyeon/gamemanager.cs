using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public DialogueController dialogueController;
    public GameObject molePrefab;
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
        InvokeRepeating(nameof(SpawnMole), 1f, spawnInterval);

        // Ÿ�̸� ����
        StartCoroutine(GameTimer());
    }

    void SpawnMole()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject mole = Instantiate(molePrefab, spawnPoint.position, Quaternion.identity);
        Destroy(mole, moleStayTime);
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
        CancelInvoke(nameof(SpawnMole));
        Debug.Log("���� ����!");

        if (!gameCleared)
            dialogueController.MinigameFailed();
        
        
        gameObject.SetActive(false);
        dialogueController.StartFirstDialogue();
    }
}
