using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance {get; private set;}

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 20;
    [SerializeField] private float tileSize = 1f;

    public int Width => width;
    public int Height => height;
    public float TileSize => tileSize;

    [Header("ラインの描画幅設定")]
    [SerializeField] public float lineWidth = 0.03f;

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

}
