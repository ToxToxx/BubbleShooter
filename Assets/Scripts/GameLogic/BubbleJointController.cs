using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BubbleJointController : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _bubbleLayer;
    [SerializeField] private int _minMatchCount = 3;
    [SerializeField] private float _proximityRadius = 0.5f;

    private Rigidbody2D _rigidbody2D;
    private bool _isConnected = false;

    private Collider2D[] _nearbyBubbles = new Collider2D[10]; 

    public delegate void BubbleAttached(GameObject bubble);
    public static event BubbleAttached OnBubbleAttached;

    public static event EventHandler OnBubbleDestroyed;
    public static event Action<List<GameObject>> OnBubblesMatchedForAnimation;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isConnected && IsInLayerMask(collision.gameObject, _wallLayer | _bubbleLayer))
        {
            if (IsInLayerMask(collision.gameObject, _wallLayer))
            {
                Debug.Log("Bounced off the wall!");
            }
            else if (IsInLayerMask(collision.gameObject, _bubbleLayer))
            {
                AttachToBubble();
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }

    private void AttachToBubble()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _rigidbody2D.isKinematic = true;

        _isConnected = true;
        Debug.Log("Bubble connected to another!");

        OnBubbleAttached?.Invoke(gameObject);
        CheckForMatches();
    }

    private void CheckForMatches()
    {
        List<GameObject> matchingBubbles = FindMatchingBubbles(gameObject);

        if (matchingBubbles.Count >= _minMatchCount)
        {
            OnBubblesMatchedForAnimation?.Invoke(matchingBubbles);

            foreach (GameObject bubble in matchingBubbles)
            {
                OnBubbleDestroyed?.Invoke(this, EventArgs.Empty);

                ScoreManager.Instance.AddScore(10);
            }

            Debug.Log("Match found! Bubbles removed.");
        }
    }

    private List<GameObject> FindMatchingBubbles(GameObject startBubble)
    {
        List<GameObject> matchingBubbles = new();
        char startColor = startBubble.GetComponent<Bubble>().ColorId;

        HashSet<GameObject> visited = new(); 
        Queue<GameObject> queue = new(); 

        queue.Enqueue(startBubble);
        visited.Add(startBubble);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            Bubble currentBubble = current.GetComponent<Bubble>();

            if (currentBubble != null && currentBubble.ColorId == startColor)
            {
                matchingBubbles.Add(current);

                int count = Physics2D.OverlapCircleNonAlloc(current.transform.position, _proximityRadius, _nearbyBubbles, _bubbleLayer);
                for (int i = 0; i < count; i++)
                {
                    GameObject nearbyBubble = _nearbyBubbles[i].gameObject;
                    if (!visited.Contains(nearbyBubble))
                    {
                        queue.Enqueue(nearbyBubble);
                        visited.Add(nearbyBubble);
                    }
                }
            }
        }

        return matchingBubbles;
    }
}
