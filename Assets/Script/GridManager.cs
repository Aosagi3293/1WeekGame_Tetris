using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int width;
    private int height;
    private float tileSize;

    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.03f;

    // 生成時の誤差を修正
    [Header("Grid Position Settings")]
    [SerializeField] private float xOffset = -0.5f;
    [SerializeField] private float yOffset = -0.5f;

    private void Start()
    {
        width = MapManager.Instance.Width;
        height = MapManager.Instance.Height;
        tileSize = MapManager.Instance.TileSize;

        CreateVerticalLines();
        CreateHorizontalLines();
    }

    // 縦ライン
    private void CreateVerticalLines()
    {
        // 線の始まりと終わりのY座標をはじめに計算しておく
        float startY = yOffset;
        float endY = (height * tileSize) + yOffset;

        for (int i = 0; i < width + 1; i++)
        {
            float xPos = (i * tileSize) + xOffset;
            
            Vector3 start = new Vector3(xPos, startY, 0);
            Vector3 end = new Vector3(xPos, endY, 0);

            CreateLine(start, end);
        }
    }

    // 横ライン
    private void CreateHorizontalLines()
    {
        // 線の始まりと終わりのX座標をはじめに計算しておく
        float startX = xOffset;
        float endX = (width * tileSize) + xOffset;

        for (int i = 0; i < height + 1; i++)
        {
            float yPos = (i * tileSize) + yOffset;

            Vector3 start = new Vector3(startX, yPos, 0);
            Vector3 end = new Vector3(endX, yPos, 0);

            CreateLine(start, end);
        }
    }

    // ラインの生成
    private void CreateLine(Vector3 start, Vector3 end)
    {
        // 新しいゲームオブジェクトを作成
        GameObject lineObj = new GameObject("GridLine");
        // このスクリプトと同じオブジェクトの子にする（インスペクターをすっきりさせるため）
        lineObj.transform.SetParent(this.transform);

        // LineRendererコンポーネントを追加
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        // 見た目の設定
        lr.material = lineMaterial;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        
        // 2D（テトリス）で手前に描画されるようにZ軸の並び順を調整（必要に応じて）
        lr.sortingOrder = 1; 

        // 座標の設定
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
