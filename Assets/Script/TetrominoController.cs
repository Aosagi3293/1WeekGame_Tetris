using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public float fallTime = 1f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= fallTime)
        {
            Move(Vector2Int.down);
            timer = 0;
        }
    }

    // 指定方向に移動
    private void Move(Vector2Int dir)
    {
        // 移動可能かチェック
        if(CanMove(dir))
        {
            transform.position += new Vector3(dir.x, dir.y, 0);
        }
        else if(dir == Vector2Int.down)
        {
            // 下に動けない = 着地
            FixToGrid();

            // 次のブロック生成
            FindObjectOfType<TetrominoSpawner>().Spawn();

            Destroy(gameObject);
        }
    }

    // 指定方向に移動できるかのチェック
    private bool CanMove(Vector2Int dir)
    {
        foreach(Transform block in transform)
        {
            Vector2Int pos = new Vector2Int(
                Mathf.RoundToInt(block.position.x) + dir.x,
                Mathf.RoundToInt(block.position.y) + dir.y
            );

            // 壁チェック
            if(!MapManager.Instance.IsInside(pos.x, pos.y))
                return false;

            // 床＆積みブロックチェック
            if(pos.y >= 0 && MapManager.Instance.IsOccupied(pos.x, pos.y))
                return false;
        }

        return true;
    }

    // 着地したブロックへの固定
    private void FixToGrid()
    {
        foreach(Transform block in transform)
        {
            Vector2Int pos = new Vector2Int(
                Mathf.RoundToInt(block.position.x),
                Mathf.RoundToInt(block.position.y)
            );

            var cell = MapManager.Instance.GetCell(pos.x, pos.y);
            cell.isFilled = true;
        }
    }
}
