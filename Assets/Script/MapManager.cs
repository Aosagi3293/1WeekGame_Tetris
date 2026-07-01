using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CellData
{
    public Vector2Int pos;
    public bool isFilled;
    public Color color;
    public Transform block;
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance {get; private set;}

    private CellData[,] grid;

    public int width => GameSettings.Instance.Width;
    public int height => GameSettings.Instance.Height;
    public float tileSize => GameSettings.Instance.TileSize;

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

    // マップ上のミノが揃ったかを確認
    public void CheckLine()
    {
        int lineCount = 0;

        // 全捜索
        for(int y = 0; y < height; y++)
        {
            bool isFull = true; // 初手に行が埋まっていると仮定

            for(int x = 0; x < width; x++)
            {
                if(!grid[x, y].isFilled)    // もし埋まっていなかったら処理しない
                {
                    isFull = false;
                    break;
                }
            }

            if(isFull)
            {
                DeleteLine(y);
                DropLines(y);

                y--;    // 段が下りる為同じ段を確認させる
                lineCount++;
            }
        }

        if(lineCount > 0)
        {
            ScoreManager.Instance.AddScore(lineCount);
        }
    }

    // 行を空にする
    private void DeleteLine(int y)
    {
        // 行を順番に削除
        for(int x = 0; x < width; x++)
        {
            CellData cell = grid[x, y];

            // 対象の[grid]があれば[null]にしてオブジェクトを破壊
            if(cell.block != null)
            {
                Destroy(cell.block.gameObject);
                cell.block = null;
            }

            // データ上でも空にしておく
            grid[x, y].isFilled = false;
        }
    }

    // 既存の行を一段下げる
    private void DropLines(int deletedY)
    {
        for(int currentY = deletedY; currentY < height - 1; currentY++)
        {
            for(int x = 0; x < width; x++)
            {
                // 上の情報を１段下へコピー
                grid[x, currentY].block = grid[x, currentY + 1].block;
                grid[x, currentY].isFilled = grid[x, currentY + 1].isFilled;

                // ブロックの見た目も落下
                if(grid[x, currentY].block != null)
                {
                    grid[x, currentY].block.position = GetWorldPosition(x, currentY);
                }
            }
        }

        for(int x = 0; x < width; x++)
        {
            grid[x, height - 1].block = null;
            grid[x, height - 1].isFilled = false;
        }
    }
}

