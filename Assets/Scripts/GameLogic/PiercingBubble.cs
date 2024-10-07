using UnityEngine;

public class PiercingBubble : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Bubble>(out var otherBubble))
        {
            Destroy(otherBubble.gameObject);

            transform.position = otherBubble.transform.position;

            Destroy(this);
        }
    }
}
