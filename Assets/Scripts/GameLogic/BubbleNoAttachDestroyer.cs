using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BubbleMatrixLoader;

public class BubbleNoAttachDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask _bubbleLayer;
    [SerializeField] private float _proximityRadius = 0.5f;
    [SerializeField] private float _destroyDelay = 2f;
    [SerializeField] private float _fallSpeed = 2f;

    private void OnEnable()
    {
        BubbleJointController.OnBubbleDestroyed += Bubble_OnBubbleDestroyed;
    }
    private void OnDisable()
    {
        BubbleJointController.OnBubbleDestroyed -= Bubble_OnBubbleDestroyed;
    }

    private void Bubble_OnBubbleDestroyed(object sender, System.EventArgs e)
    {
        Invoke(nameof(CheckAndDestroy), _destroyDelay);
    }

    private void CheckAndDestroy()
    {
        if (!IsConnectedToOtherBubbles())
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -_fallSpeed);
            Debug.Log("Bubble destroyed because it is not attached to others.");
        }
    }

    private bool IsConnectedToOtherBubbles()
    {
        Collider2D[] nearbyBubbles = Physics2D.OverlapCircleAll(transform.position, _proximityRadius, _bubbleLayer);
        return nearbyBubbles.Length > 1;
    }
}


