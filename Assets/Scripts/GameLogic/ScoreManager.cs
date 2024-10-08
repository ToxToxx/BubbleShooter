using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;  

    private int _currentScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
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
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }
}
