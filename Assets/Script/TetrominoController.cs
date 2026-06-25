using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TetrominoController : MonoBehaviour
{
    public float fallTime = 1f;
    private float timer;
    private bool move = true;

    private float softDropTimer;
    private float softDropInterval = 0.05f;

    public Vector2Int[] cells = new Vector2Int[4];
    
    Vector2Int[] kickTests =
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(-2, 0),
    };

    private void Update()
    {
        InputControl();

        timer += Time.deltaTime;

        if(timer >= fallTime)
        {
            Move(Vector2Int.down);
            timer = 0;
        }
    }

    // 下方向に移動
    private void Move(Vector2Int dir)
    {
        // 移動可能かチェック
        if(CanMove(dir))
        {
            transform.position += new Vector3(dir.x, dir.y, 0);
        }
        else if(dir == Vector2Int.down && move == true)
        {
            // 下に動けない = 着地
            FixToGrid();

            // 次のブロック生成
            FindObjectOfType<TetrominoSpawner>().Spawn();

            move = false;

            Destroy(this);
        }
    }

    // キーの入力
    private void InputControl()
    {
        if(Keyboard.current.leftArrowKey.wasPressedThisFrame) Move(Vector2Int.left);
        if(Keyboard.current.rightArrowKey.wasPressedThisFrame) Move(Vector2Int.right);
        if(Keyboard.current.upArrowKey.wasPressedThisFrame) Rotate();
        if(Keyboard.current.downArrowKey.isPressed)
        {
            softDropTimer += Time.deltaTime;

            if(softDropTimer >= softDropInterval)
            {
                Move(Vector2Int.down);
                softDropTimer = 0;
            }
        }
        else
        {
            softDropTimer = 0;
        }
        if(Keyboard.current.spaceKey.isPressed) HardDrop();
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

    // 回転処理
    private void Rotate()
    {
        Vector2Int[] rotatedCells = GetRotatedCells();

        foreach(var kick in kickTests)
        {
            if(CanPlace(rotatedCells, kick))
            {
                ApplyRotation(rotatedCells, kick);
                return;
            }
        }
    }

    // 回転後のミノの計算
    private Vector2Int[] GetRotatedCells()
    {
        Vector2Int[] rotated = new Vector2Int[cells.Length];

        for(int i = 0; i < cells.Length; i++)
        {
            int x = cells[i].x;
            int y = cells[i].y;

            rotated[i] = new Vector2Int(-y, x);
        }

        return rotated;
    }

    // 回転後のミノが置けるかの計算
    private bool CanPlace(Vector2Int[] testCells, Vector2Int offset)
    {
        foreach(var cell in testCells)
        {
            int x =
                Mathf.RoundToInt(transform.position.x)
                + cell.x
                + offset.x;

            int y =
                Mathf.RoundToInt(transform.position.y)
                + cell.y
                + offset.y;

            if(!MapManager.Instance.IsInside(x, y))
                return false;

            if(MapManager.Instance.IsOccupied(x, y))
                return false;
        }

        return true;
    }

    // 回転の適用
    private void ApplyRotation(Vector2Int[] rotatedCells, Vector2Int kick)
    {
        cells = rotatedCells;

        transform.position += new Vector3(kick.x, kick.y, 0);

        UpdateVisual();
    }

    // ミノの並べ替え
    private void UpdateVisual()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector2(cells[i].x, cells[i].y);
        }
    }

    // ミノを一番下まで落とす
    private void HardDrop()
    {
        while(CanMove(Vector2Int.down))
        {
            transform.position += Vector3.down;
        }

        FixToGrid();
    }
}
