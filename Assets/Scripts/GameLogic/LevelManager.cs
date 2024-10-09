using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = 2f;
    private List<GameObject> _allBubbles = new();
    private List<GameObject> _topRowBubbles = new();  
    [SerializeField] private float _winPercentage = 0.3f;
    [SerializeField] private int _initialTopRowCount;
    [SerializeField] private int _winBubblesGoal;


    public static event EventHandler OnGameWon;

    private void OnEnable()
    {
        BubbleMatrixLoader.OnBubblesCreated += OnBubblesCreated;
        BubbleMatrixLoader.OnTopRowBubblesCreated += OnTopRowBubblesCreated;  
    }

    private void OnDisable()
    {
        BubbleMatrixLoader.OnBubblesCreated -= OnBubblesCreated;
        BubbleMatrixLoader.OnTopRowBubblesCreated -= OnTopRowBubblesCreated;
    }

    private void Update()
    {
        RemoveDestroyedTopRowBubbles();
        if (_topRowBubbles.Count <= _winBubblesGoal)
        {
            GameWon();
        }
    }
    private void OnBubblesCreated(List<GameObject> bubbles)
    {
        _allBubbles = bubbles;

        foreach (var bubble in _allBubbles)
        {
            bubble.transform.SetParent(transform);
        }
    }

    private void OnTopRowBubblesCreated(List<GameObject> topRowBubbles)
    {
        _topRowBubbles = topRowBubbles;  
        _initialTopRowCount = _topRowBubbles.Count;
        _winBubblesGoal = Mathf.CeilToInt(_initialTopRowCount * _winPercentage);
    }

    private void GameWon()
    {
        Debug.Log("Game Won!");
        TriggerRemainingBubblesFall();
        OnGameWon?.Invoke(this, EventArgs.Empty);
    }

    private void TriggerRemainingBubblesFall()
    {
        foreach (var bubble in _allBubbles)
        {
            if (bubble != null)
            {
                Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();
                FixedJoint2D bubbleJoint = bubble.GetComponent<FixedJoint2D>();

                if (bubbleRb != null && bubbleRb.isKinematic)
                {
                    if (bubbleJoint != null)
                    {
                        Destroy(bubbleJoint);
                    }
                    bubbleRb.isKinematic = false;
                    bubbleRb.velocity = new Vector2(0, -_fallSpeed);
                }
            }
        }

        Debug.Log("Remaining bubbles are falling.");
    }

    private void RemoveDestroyedTopRowBubbles()
    {
        _topRowBubbles.RemoveAll(bubble => bubble == null);
    }
}
