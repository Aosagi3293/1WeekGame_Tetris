using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}

    public int score {get; private set;}

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

    // スコア加算
    public void AddScore(int lineCount)
    {
        switch(lineCount)
        {
            case 1:
                score += 10;
                break;
            case 2:
                score += 30;
                break;
            case 3:
                score += 50;
                break;
            case 4:
                score += 80;
                break;
        }

        Debug.Log($"Score : {score}");
    }
}
