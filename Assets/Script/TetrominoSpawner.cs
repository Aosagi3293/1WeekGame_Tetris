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

    [SerializeField] private TetrominoType type;

    private void Start()
    {
        Spawn();
    }

    // ブロックの生成
    public void Spawn()
    {
        type = (TetrominoType)Random.Range(0, 7);
        Vector2Int[] shape = TetrominoData.Shapes[type];

        GameObject parent = new GameObject("Tetromino");

        TetrominoController controller = parent.AddComponent<TetrominoController>();

        controller.cells = (Vector2Int[])shape.Clone();

        foreach(var pos in shape)
        {
            GameObject block = Instantiate(blockPrefab, parent.transform);
            block.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        }

        parent.transform.position = MapManager.Instance.GetWorldPosition(5, 18);
    }
}
