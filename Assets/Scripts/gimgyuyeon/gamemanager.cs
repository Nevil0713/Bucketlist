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

    public float gameDuration = 30f; // 게임 제한 시간 (초)
    public TextMeshProUGUI timerText; // 남은 시간을 표시할 UI

    public bool gameCleared = false;

    private float remainingTime;

    void Start()
    {
        remainingTime = gameDuration;

        // 두더지 생성 반복
        InvokeRepeating(nameof(SpawnMole), 1f, spawnInterval);

        // 타이머 시작
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

        // 타이머 종료
        timerText.text = "Time: 0";
        CancelInvoke(nameof(SpawnMole));
        Debug.Log("게임 종료!");

        if (!gameCleared)
            dialogueController.MinigameFailed();
        
        
        gameObject.SetActive(false);
        dialogueController.StartFirstDialogue();
    }
}
