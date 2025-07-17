using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Teto_Stage : MonoBehaviour
{
    //Object linked with U6 Editor
    [Header("Editor Objects")]
    public GameObject dialogueManager;
    public GameObject dialogCanvas;
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tet_blockNode;
    public Transform previewNode;
    public Transform boardOrigin;

    [Header("Panel/UI Settings")]
    public GameObject gameoverPanel;
    public GameObject gamedonePanel;
    public GameObject UI_score;
    public TMP_Text Score;
    public TMP_Text Target;
    public Image Gauge;

    //Setting Game Space...?
    [Header("Game Settings")]
    [Range(4, 40)]
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCycle = 1.0f;

    private int halfWidth;
    private int halfHeight;

    private float nextFallTime;

    //public Teto_Player_Anim Terto;
    public PlayerFaceController playerFace;
    public Teto_Timer Tetime;

    private int scoreVal = 0;
    private int targetVal = 2000;

    private int indexVal = -1;

    private bool m_gameStarted = false;
    private bool m_gameEnded = false;

    private void Start()
    {
        ScreenFader.FadeOut();
        Invoke("StartStage", 1);
    }

    void StartStage()
    {
        Gauge.fillAmount = 0.1f;
        gameoverPanel.SetActive(false);
        gamedonePanel.SetActive(false);

        halfWidth = (int)(boardWidth * 0.5f);
        halfHeight = (int)(boardHeight * 0.5f);

        nextFallTime = Time.time + fallCycle;

        CreateBackground();

        for (int i = 0; i < boardHeight; ++i)
        {
            var col = new GameObject((boardHeight - i - 1).ToString());
            col.transform.position = boardOrigin.position + new Vector3(0, halfHeight - i, 0);
            col.transform.parent = boardNode;
        }

        CreateBlock();
        m_gameStarted = true;
    }

    void Update()
    {
        if (!m_gameStarted)
            return;

        Vector3 moveDir = Vector3.zero;
        bool isRotate = false;

        //player control
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir.x = -1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir.x = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotate = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir.y = -1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (moveTeto(Vector3.down, false))
            {
            }
        }

        //컨티뉴
        if (gameoverPanel.activeSelf || gamedonePanel.activeSelf)
        {
            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        //fall automatically
        if (Time.time > nextFallTime)
        {
            nextFallTime = Time.time + fallCycle;
            moveDir = Vector3.down;
            isRotate = false;
        }

        if (moveDir != Vector3.zero || isRotate)
        {
            moveTeto(moveDir, isRotate);
        }
    }

    private void Awake()
    {
        //if (Terto == null)
        //{
        //    Terto = FindFirstObjectByType<Teto_Player_Anim>();
        //}

        if (Tetime == null)
        {
            Tetime = FindFirstObjectByType<Teto_Timer>();
        }
    }

    //active to end the game also active animation
    public void gameOver_setters()
    {
        if (m_gameEnded)
            return;
        m_gameEnded = true;

        bool isWin = scoreVal >= targetVal;
        bool isOver = Tetime.GameTime <= 0;
        
        if (isOver)
        {
            playerFace.SetPlayerFace(4);
            gameoverPanel.SetActive(true);
            gamedonePanel.SetActive(false);
        }
        else if (isWin)
        {
            playerFace.SetPlayerFace(2);
            gameoverPanel.SetActive(false);
            gamedonePanel.SetActive(true);
        }
        else if (!isWin)
        {
            playerFace.SetPlayerFace(3);
            gameoverPanel.SetActive(true);
            gamedonePanel.SetActive(false);
        }
        else
        {
            gameoverPanel.SetActive(false);
            gamedonePanel.SetActive(false);
        }

        StartCoroutine(TurnDialogueOn(isWin));
    }
    IEnumerator TurnDialogueOn(bool isWin)
    {
        yield return new WaitForSeconds(1);
        UI_score.SetActive(false);
        yield return new WaitForSeconds(1);

        if(!isWin)
        {
            Debug.Log("Fail!");
            dialogueManager.GetComponent<DialogueController>().MinigameFailed();
        }

        dialogueManager.GetComponent<DialogueController>().StartFirstDialogue();
        gameObject.SetActive(false);
    }


    bool moveTeto(Vector3 moveDir, bool isRotate)
    {
        Vector3 oldPos = tet_blockNode.transform.position;
        Quaternion oldRot = tet_blockNode.transform.rotation;

        tet_blockNode.transform.position += moveDir;
        if (isRotate)
        {
            tet_blockNode.transform.rotation *= Quaternion.Euler(0, 0, 90);
        }

        if (!canMoveTo(tet_blockNode))
        {
            tet_blockNode.transform.position = oldPos;
            tet_blockNode.transform.rotation = oldRot;

            if ((int)moveDir.y == -1 && (int)moveDir.x == 0 && isRotate == false)
            {
                AddToBoard(tet_blockNode);
                CheckBoardColumn();
                CreateBlock();

                if (!canMoveTo(tet_blockNode))
                {
                    if (scoreVal != targetVal)
                    {
                        Invoke("gameOver_setters", 1.0f);
                    }
                }
            }

            return false;
        }
        return true;
    }

    void AddToBoard(Transform root)
    {
        while (root.childCount > 0)
        {
            var node = root.GetChild(0);
            Vector3 localPos = node.position - boardOrigin.position;

            int x = Mathf.RoundToInt(localPos.x + halfWidth);
            int y = Mathf.RoundToInt(localPos.y + halfHeight - 1);

            node.parent = boardNode.Find(y.ToString());
            node.name = x.ToString();
        }
    }


    void CheckBoardColumn()
    {
        bool isCleared = false;

        int lineCount = 0;

        foreach (Transform column in boardNode)
        {
            if (column.childCount == boardWidth)
            {
                foreach (Transform tile in column)
                {
                    Destroy(tile.gameObject);
                }

                column.DetachChildren();
                isCleared = true;
                lineCount++;
            }
        }
        //clear the line get the score. also active animation.
        if (lineCount != 0)
        {
            playerFace.SetPlayerFace(1);
            IEnumerator ResetPlayerFace()
            {
                yield return new WaitForSeconds(1);
                playerFace.SetPlayerFace(0);
            }
            StartCoroutine(ResetPlayerFace());

            scoreVal += 200;
            Score.text = "" + scoreVal;
            ScoreGauge();
        }

        //winning target
        if (lineCount != 0)
        {
            if (scoreVal == targetVal || scoreVal == 2000)
            {
                scoreVal = 2000;
                Invoke("gameOver_setters", 1.0f);
            }
        }

        if (isCleared)
        {
            for (int i = 1; i < boardNode.childCount; ++i)
            {
                var column = boardNode.Find(i.ToString());

                if (column.childCount == 0)
                    continue;

                int emptyCol = 0;
                int j = i - 1;

                while (j >= 0)
                {
                    if (boardNode.Find(j.ToString()).childCount == 0)
                    {
                        emptyCol++;
                    }
                    j--;
                }
                if (emptyCol > 0)
                {
                    var targetColumn = boardNode.Find((i - emptyCol).ToString());

                    while (column.childCount > 0)
                    {
                        Transform tile = column.GetChild(0);
                        tile.parent = targetColumn;
                        tile.transform.position += new Vector3(0, -emptyCol, 0);
                    }
                    column.DetachChildren();
                }
            }
        }
    }

    void StopHappyWrapper()
    {
        //if (Terto != null)
        //    Terto.StopHappy();
    }

    //score gauge bar
    void ScoreGauge()
    {
        Gauge.fillAmount += 0.06f;
    }


    //in board move checker
    bool canMoveTo(Transform root)
    {
        for (int i = 0; i < root.childCount; ++i)
        {
            var node = root.GetChild(i);
            Vector3 localPos = node.position - boardOrigin.position;

            int x = Mathf.RoundToInt(localPos.x + halfWidth);
            int y = Mathf.RoundToInt(localPos.y + halfHeight - 1.0f);

            if (x < 0 || x >= boardWidth || y < 0)
                return false;

            var column = boardNode.Find(y.ToString());
            if (column != null && column.Find(x.ToString()) != null)
                return false;
        }

        return true;
    }


    //Creates Tile code
    Tile CreateTile(Transform parent, Vector2 localPosition, Color color, int order = 1)
    {
        var go = Instantiate(tilePrefab);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;

        var tile = go.GetComponent<Tile>();
        tile.color = color;
        tile.sortingOrder = order;

        return tile;
    }


    //Creates BG
    void CreateBackground()
    {
        Color color = Color.gray;

        color.a = 0.6f;
        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        color.a = 1.0f;
        for (int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        for (int x = -halfWidth - 1; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }

        backgroundNode.position = boardOrigin.position;
    }



    //creates tet_block
    void CreateBlock()
    {
        int index = (indexVal == -1) ? Random.Range(0, 7) : indexVal;
        Color32 color = Color.white;

        tet_blockNode.rotation = Quaternion.identity;
        tet_blockNode.position = boardOrigin.position + new Vector3(0, halfHeight - 1, 0);

        switch (index)
        {
            //I 모양
            case 0:
                color = new Color32(115, 251, 253, 255);

                CreateTile(tet_blockNode, new Vector2(-2f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(-1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);

                break;

            //J 모양
            case 1:
                color = new Color32(0, 33, 245, 255);

                CreateTile(tet_blockNode, new Vector2(-1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(-1f, 1f), color);

                break;

            //L 모양
            case 2:
                color = new Color32(243, 168, 59, 255);

                CreateTile(tet_blockNode, new Vector2(-1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 1f), color);

                break;

            //O 모양
            case 3:
                color = new Color32(255, 253, 84, 255);

                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 1f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 1f), color);

                break;

            //S 모양
            case 4:
                color = new Color32(117, 250, 76, 255);

                CreateTile(tet_blockNode, new Vector2(-1f, -1f), color);
                CreateTile(tet_blockNode, new Vector2(0f, -1f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);

                break;

            //T 모양
            case 5:
                color = new Color32(155, 47, 246, 255);

                CreateTile(tet_blockNode, new Vector2(-1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 1f), color);

                break;

            //Z 모양
            case 6:
                color = new Color32(235, 51, 35, 255);

                CreateTile(tet_blockNode, new Vector2(-1f, 1f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 1f), color);
                CreateTile(tet_blockNode, new Vector2(0f, 0f), color);
                CreateTile(tet_blockNode, new Vector2(1f, 0f), color);

                break;
        }
        CreatePre();

    }

    void CreatePre()
    {
        foreach (Transform tile in previewNode)
        {
            Destroy(tile.gameObject);
        }
        previewNode.DetachChildren();

        indexVal = Random.Range(0, 7);
        Color32 color = Color.white;

        previewNode.position = boardOrigin.position + new Vector3(halfWidth + 6.2f, halfHeight - 5, 0);

        switch (indexVal)
        {
            //I 모양
            case 0:
                color = new Color32(115, 251, 253, 255);

                CreateTile(previewNode, new Vector2(-2f, 0f), color);
                CreateTile(previewNode, new Vector2(-1f, 0f), color);
                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);

                break;

            //J 모양
            case 1:
                color = new Color32(0, 33, 245, 255);

                CreateTile(previewNode, new Vector2(-1f, 0f), color);
                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);
                CreateTile(previewNode, new Vector2(-1f, 1f), color);

                break;

            //L 모양
            case 2:
                color = new Color32(243, 168, 59, 255);

                CreateTile(previewNode, new Vector2(-1f, 0f), color);
                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 1f), color);

                break;

            //O 모양
            case 3:
                color = new Color32(255, 253, 84, 255);

                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);
                CreateTile(previewNode, new Vector2(0f, 1f), color);
                CreateTile(previewNode, new Vector2(1f, 1f), color);

                break;

            //S 모양
            case 4:
                color = new Color32(117, 250, 76, 255);

                CreateTile(previewNode, new Vector2(-1f, -1f), color);
                CreateTile(previewNode, new Vector2(0f, -1f), color);
                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);

                break;

            //T 모양
            case 5:
                color = new Color32(155, 47, 246, 255);

                CreateTile(previewNode, new Vector2(-1f, 0f), color);
                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);
                CreateTile(previewNode, new Vector2(0f, 1f), color);

                break;

            //Z 모양
            case 6:
                color = new Color32(235, 51, 35, 255);

                CreateTile(previewNode, new Vector2(-1f, 1f), color);
                CreateTile(previewNode, new Vector2(0f, 1f), color);
                CreateTile(previewNode, new Vector2(0f, 0f), color);
                CreateTile(previewNode, new Vector2(1f, 0f), color);

                break;
        }
    }
}