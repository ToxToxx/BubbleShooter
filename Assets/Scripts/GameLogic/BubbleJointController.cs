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

    public delegate void BubbleAttached(GameObject bubble);
    public static event BubbleAttached OnBubbleAttached;

    public static event EventHandler OnBubbleDestroyed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject, _wallLayer))
        {
            Debug.Log("Bounced off the wall!");
        }

        if (!_isConnected && IsInLayerMask(collision.gameObject, _bubbleLayer))
        {
            AttachToBubble();
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
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
            foreach (GameObject bubble in matchingBubbles)
            {
                Destroy(bubble);
                OnBubbleDestroyed?.Invoke(this, EventArgs.Empty);
            }

            Debug.Log("Match found! Bubbles removed.");
        }
    }

    private List<GameObject> FindMatchingBubbles(GameObject startBubble)
    {
        List<GameObject> matchingBubbles = new();
        char startColor = startBubble.GetComponent<Bubble>().ColorId;

        Collider2D[] nearbyBubbles = Physics2D.OverlapCircleAll(startBubble.transform.position, _proximityRadius, _bubbleLayer);
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

                nearbyBubbles = Physics2D.OverlapCircleAll(current.transform.position, _proximityRadius, _bubbleLayer);
                foreach (var nearbyBubbleCollider in nearbyBubbles)
                {
                    GameObject nearbyBubble = nearbyBubbleCollider.gameObject;
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
