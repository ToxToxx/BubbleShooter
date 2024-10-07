using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBottomTreshold : MonoBehaviour
{
    [SerializeField] private BubbleSpawner _bubbleSpawner;
    [SerializeField] private LayerMask _bubbleLayerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _bubbleLayerMask) != 0)
        {
            _bubbleSpawner.ReleaseBubble(collision.gameObject.GetComponent<Bubble>());
        } 
    }
}
