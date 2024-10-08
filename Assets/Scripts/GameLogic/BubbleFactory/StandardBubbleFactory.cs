using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBubbleFactory : IBubbleFactory
{
    private GameObject _bubblePrefab;

    public StandardBubbleFactory(GameObject bubblePrefab)
    {
        _bubblePrefab = bubblePrefab;
    }

    public Bubble CreateBubble()
    {
        GameObject bubbleInstance = Object.Instantiate(_bubblePrefab);
        return bubbleInstance.GetComponent<Bubble>();
    }
}