using UnityEngine;
using System.Collections.Generic;

public enum TetrominoType
{
    I, O, T, S, Z, J, L
}

public static class TetrominoData
{
    public static readonly Dictionary<TetrominoType, Vector2Int[]> Shapes =
        new Dictionary<TetrominoType, Vector2Int[]>
        {
            { TetrominoType.I, new []{
                new Vector2Int(-1, 0), new Vector2Int(0, 0),
                new Vector2Int(1, 0), new Vector2Int(2, 0),
            }},
            { TetrominoType.O, new []{
                new Vector2Int(0, 0), new Vector2Int(1, 0),
                new Vector2Int(0, 1), new Vector2Int(1, 1),
            }},
            { TetrominoType.T, new []{
                new Vector2Int(-1, 0), new Vector2Int(0, 0),
                new Vector2Int(0, 1), new Vector2Int(1, 0),
            }},
            { TetrominoType.S, new []{
                new Vector2Int(-1, 0), new Vector2Int(0, 0),
                new Vector2Int(0, 1), new Vector2Int(1, 1),
            }},
            { TetrominoType.Z, new []{
                new Vector2Int(-1, 1), new Vector2Int(0, 1),
                new Vector2Int(0, 0), new Vector2Int(1, 0),
            }},
            { TetrominoType.J, new []{
                new Vector2Int(-1, 1), new Vector2Int(-1, 0),
                new Vector2Int(0, 0), new Vector2Int(1, 0),
            }},
            { TetrominoType.L, new []{
                new Vector2Int(-1, 0), new Vector2Int(0, 0),
                new Vector2Int(1, 0), new Vector2Int(1, 1),
            }},
        };
}

public class TetrominoSpawner : MonoBehaviour
{
    public GameObject blockPrefab;

    private float tileSize;

    [SerializeField] private TetrominoType type;

    private void Start()
    {
        tileSize = GameSettings.Instance.TileSize;
        Spawn();
    }

    // ブロックの生成
    public void Spawn()
    {
        // タイプの決定と形状データの取得
        type = (TetrominoType)Random.Range(0, 7);
        Vector2Int[] shape = TetrominoData.Shapes[type];

        // 空の親オブジェクトを生成し、先に初期位置へ配置
        GameObject parent = new GameObject("Tetromino");
        parent.transform.position = MapManager.Instance.GetWorldPosition(5, 18);

        // コントローラーの追加とデータのディープコピー
        TetrominoController controller = parent.AddComponent<TetrominoController>();
        controller.cells = (Vector2Int[])shape.Clone();

        // 子オブジェクトを生成して配置
        foreach(var pos in shape)
        {
            GameObject block = Instantiate(blockPrefab, parent.transform);
            block.transform.localPosition = new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
            block.transform.localScale = Vector3.one * tileSize;
        }
    }
}
