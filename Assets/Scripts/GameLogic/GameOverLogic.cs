using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLogic : MonoBehaviour
{
    [SerializeField] private int _spawnedBubblesTillGameOver = 30;


    public static event EventHandler OnGameLoose;
    private void OnEnable()
    {
        BubbleSpawner.OnBubbleHadSpawned += AddBubble;
    }

    private void OnDisable()
    {
        BubbleSpawner.OnBubbleHadSpawned -= AddBubble;
    }

    public void AddBubble()
    {
        _spawnedBubblesTillGameOver--;
        IsGameOver();
    }

    public void IsGameOver()
    {
        if(_spawnedBubblesTillGameOver <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        OnGameLoose?.Invoke(this, EventArgs.Empty);
    }

    public int GetLeftBubbles()
    {
        return _spawnedBubblesTillGameOver;
    }
}
