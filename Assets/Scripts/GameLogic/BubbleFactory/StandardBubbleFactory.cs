using UnityEngine;

public class StandardBubbleFactory : IBubbleFactory
{
    private readonly GameObject _bubblePrefab;

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