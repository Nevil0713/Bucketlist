using UnityEngine;

public class Tet_Stage : MonoBehaviour
{
    //Object linked with U6 Editor
    [Header("Editor Objects")]
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tet_blockNode;

    //Setting Game Space...?
    [Header("Game Settings")]
    [Range(4, 40)]
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCycle = 0.5f;

    private int halfWidth;
    private int halfHeight;

    private float nextFallTime;

    private void Start()
    {
        halfWidth = (int)(boardWidth * 0.5f);
        halfHeight = (int)(boardHeight * 0.5f);

        nextFallTime = Time.time + fallCycle;

        CreateBackground();
        CreateBlock();
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        bool isRotate = false;

        //player control
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir.x = -0.5f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir.x = 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotate = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir.y = -0.5f;
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

    bool moveTeto(Vector3 moveDir, bool isRotate)
    {
        tet_blockNode.transform.position += moveDir;
        if (isRotate)
        {
            tet_blockNode.transform.rotation *= Quaternion.Euler(0, 0, 90);
        }

        return true;
    }

    //Creates Tile code
    Tile CreateTile(Transform parent, Vector2 position, Color color, int order = 1)
    {
        var go = Instantiate(tilePrefab);
        go.transform.parent = parent;
        go.transform.localPosition = position * 0.5f;

        var tile = go.GetComponent<Tile>();
        tile.color = color;
        tile.sortingOrder = order;

        return tile;
    }

    //Creates BG
    void CreateBackground()
    {
        Color color = Color.gray;

        //board
        color.a = 0.8f;
        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for(int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        //lines Left and Right
        color.a = 1.0f;
        for (int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }
        
        //under line
        for(int x = -halfWidth - 1; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }

    //creates tet_block
    void CreateBlock()
    {
        int index = Random.Range(0, 7);
        Color32 color = Color.white;

        tet_blockNode.rotation = Quaternion.identity;
        tet_blockNode.position = new Vector2(0, halfHeight - 1);

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

    }

}