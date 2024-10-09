using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private void Update()
    {
        _bestScoreText.text = "BEST SCORE: " + ScoreManager.Instance.GetBestScore().ToString();
    }
}
