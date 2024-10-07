using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = 2f;
    private List<GameObject> _allBubbles = new();
    [SerializeField] private int _initialBubbleCount = 0;
    [SerializeField] private float _winPercentage = 0.3f;
    [SerializeField] private int _winBubblesGoal;

    private void OnEnable()
    {
        BubbleMatrixLoader.OnBubblesCreated += OnBubblesCreated;
        BubbleJointController.OnBubbleDestroyed += Bubble_OnBubbleDestroyed;
    }

    private void Bubble_OnBubbleDestroyed(object sender, System.EventArgs e)
    {
        Debug.Log("Calculating Bubbles");
        _initialBubbleCount--;

        if (_initialBubbleCount  <= _winBubblesGoal)
        {
            GameWon();
        }
    }

    private void OnDisable()
    {
        BubbleMatrixLoader.OnBubblesCreated -= OnBubblesCreated;
        BubbleJointController.OnBubbleDestroyed -= Bubble_OnBubbleDestroyed;
    }

    private void OnBubblesCreated(List<GameObject> bubbles)
    {
        _allBubbles = bubbles;
        _initialBubbleCount = _allBubbles.Count;
        _winBubblesGoal = (int)(_initialBubbleCount * _winPercentage);

        foreach (var bubble in _allBubbles)
        {
            bubble.transform.SetParent(transform);
        }
    }

    private void GameWon()
    {
        Debug.Log("Game Won!");
        TriggerRemainingBubblesFall(); 
    }

    private void TriggerRemainingBubblesFall()
    {
        foreach (var bubble in _allBubbles)
        {
            if (bubble != null)
            {
                Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();
                if (bubbleRb != null && bubbleRb.isKinematic)
                {
                    bubbleRb.isKinematic = false;
                    bubbleRb.velocity = new Vector2(0, -_fallSpeed);
                }
            }
        }

        Debug.Log("Remaining bubbles are falling.");
    }
}
