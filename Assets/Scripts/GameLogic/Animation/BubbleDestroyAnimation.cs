using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDestroyAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        BubbleJointController.OnBubblesMatchedForAnimation += HandleBubblesMatchedForAnimation;
        PiercingBubble.OnBubblePierced += HandleBubbleMatchedForAnimation;
    }

    private void OnDisable()
    {
        BubbleJointController.OnBubblesMatchedForAnimation -= HandleBubblesMatchedForAnimation;
        PiercingBubble.OnBubblePierced -= HandleBubbleMatchedForAnimation;
    }

    private void HandleBubblesMatchedForAnimation(List<GameObject> bubbles)
    {
        foreach (var bubble in bubbles)
        {
            StartCoroutine(PlayDestroyAnimation(bubble));
        }
    }

    private void HandleBubbleMatchedForAnimation(GameObject bubble)
    {
        StartCoroutine(PlayDestroyAnimation(bubble));
    }

    private IEnumerator PlayDestroyAnimation(GameObject bubble)
    {
        float duration = 0.5f;  
        float elapsed = 0f;
        Vector3 originalScale = bubble.transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, elapsed / duration);
            bubble.transform.localScale = originalScale * scale;
            yield return null;
        }
        Destroy(bubble);
    }
}
