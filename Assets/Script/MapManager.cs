using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CellData
{
    public Vector2Int pos;
    public bool isFilled;
    public Color color;
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance {get; private set;}

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 20;
    [SerializeField] private float tileSize = 1;

    private CellData[,] grid;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MapBuild();
    }

    // グリッドの生成
    private void MapBuild()
    {
        grid = new CellData[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                CellData cell = new CellData();
                cell.pos = new Vector2Int(x, y);
                cell.isFilled = false;
                cell.color = Color.white;

                grid[x, y] = cell;
            }
        }
    }

    // セルのワールド座標を返す
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x * tileSize, y * tileSize);
    }

    // グリッド上の座標を返す
    public CellData GetCell(int x, int y)
    {
        return grid[x, y];
    }

    // ブロックの下が壁でないことをチェック
    public bool IsInside(int x, int y)
    {
        return x >= 0 && x < width && y >= 0;
    }

    // ブロックの下がブロックでないことをチェック
    public bool IsOccupied(int x, int y)
    {
        return grid[x, y].isFilled;
    }
}

