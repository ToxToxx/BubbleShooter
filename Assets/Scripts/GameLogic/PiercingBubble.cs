using System;
using UnityEngine;

public class PiercingBubble : MonoBehaviour
{
    public static Action<GameObject> OnBubblePierced;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Bubble>(out var otherBubble))
        {
            OnBubblePierced?.Invoke(otherBubble.gameObject);

            transform.position = otherBubble.transform.position;

            Destroy(this);
        }
    }
}
