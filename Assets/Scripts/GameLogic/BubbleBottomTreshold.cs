using UnityEngine;

public class BubbleBottomThreshold : MonoBehaviour
{
    [SerializeField] private BubbleSpawner _bubbleSpawner;
    [SerializeField] private LayerMask _bubbleLayerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, _bubbleLayerMask))
        {
            if (collision.gameObject.TryGetComponent<Bubble>(out var bubble))
            {
                _bubbleSpawner.ReleaseBubble(bubble);
            }
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}
