using UnityEngine;

public class PiercingBubble : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is a bubble
        if (collision.gameObject.TryGetComponent<Bubble>(out var otherBubble))
        {
            // Destroy the other bubble
            Destroy(otherBubble.gameObject);

            // Move this bubble to the other bubble's position
            transform.position = otherBubble.transform.position;

            // Disable this script after the first collision
            Destroy(this);
        }
    }
}
