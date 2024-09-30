using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int HighScore { get; private set; }
    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void SaveHighScore(int newHighScore)
    {
        HighScore = newHighScore;
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.Save();
    }

    public void IncreaseScore(int amount)
    {
        Score += amount;
        if (Score > HighScore)
        {
            SaveHighScore(Score);
        }
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
