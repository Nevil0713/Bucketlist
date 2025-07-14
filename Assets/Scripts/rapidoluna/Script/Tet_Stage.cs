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
    public float fallCycle = 1.0f;

    private int halfWidth;
    private int halfHeight;

    private void Start()
    {
        halfWidth = (int)(boardWidth * 0.5f);
        halfHeight = (int)(boardHeight * 0.5f);

        CreateBackground();
    }


    //Creates Tile code
    Tile CreateTile(Transform parent, Vector2 position, Color color, int order = 1)
    {
        var go = Instantiate(tilePrefab);
        go.transform.parent = parent;
        go.transform.localPosition = position * 0.55f;

        var tile = go.GetComponent<Tile>();
        tile.color = color;
        tile.sortingOrder = order;

        return tile;
    }

    //Creates BG
    void CreateBackground()
    {
        Color color = Color.grey;

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
}