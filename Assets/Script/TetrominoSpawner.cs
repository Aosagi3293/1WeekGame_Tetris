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
                new Vector2Int(1, 0), new Vector2Int(1, 1),
            }},
            { TetrominoType.S, new []{
                new Vector2Int(-1, 0), new Vector2Int(0, 0),
                new Vector2Int(1, 0), new Vector2Int(1, 1),
            }},
            { TetrominoType.Z, new []{
                new Vector2Int(-1, 1), new Vector2Int(0, 1),
                new Vector2Int(0, 0), new Vector2Int(0, 1),
            }},
            { TetrominoType.J, new []{
                new Vector2Int(-1, 1), new Vector2Int(-1, 0),
                new Vector2Int(0, 0), new Vector2Int(0, 1),
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

    private void Start()
    {
        Spawn();
    }

    // ブロックの生成
    public void Spawn()
    {
        TetrominoType type = (TetrominoType)Random.Range(0, 7);
        Vector2Int[] shape = TetrominoData.Shapes[type];

        GameObject parent = new GameObject("Tetromino");

        TetrominoController controller = parent.AddComponent<TetrominoController>();

        foreach(var pos in shape)
        {
            GameObject block = Instantiate(blockPrefab, parent.transform);
            block.transform.position = MapManager.Instance.GetWorldPosition(pos.x + 5, pos.y + 18);
        }
    }
}
