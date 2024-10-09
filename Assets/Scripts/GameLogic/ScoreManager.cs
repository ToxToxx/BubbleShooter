using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int _currentScore;
    private int _highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadBestScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        Debug.Log($"Score added: {points}. Current score: {_currentScore}");

        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            SaveBestScore();
            Debug.Log($"New high score: {_highScore}");
        }
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }

    public int GetBestScore()
    {
        return _highScore;
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", _highScore);
        PlayerPrefs.Save();
    }

    private void LoadBestScore()
    {
        _highScore = PlayerPrefs.GetInt("BestScore", 0); 
    }
}
