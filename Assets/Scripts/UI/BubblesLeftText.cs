using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubblesLeftText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bubblesLeftText;
    [SerializeField] private GameOverLogic _gameOverLogic;

    private void Update()
    {
        _bubblesLeftText.text = _gameOverLogic.GetLeftBubbles().ToString();
    }
}
