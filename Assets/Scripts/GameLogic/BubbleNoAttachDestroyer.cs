using UnityEngine;

public class BubbleNoAttachDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask _bubbleLayer;
    [SerializeField] private float _proximityRadius = 0.5f;
    [SerializeField] private float _destroyDelay = 2f;
    [SerializeField] private float _fallSpeed = 2f;

    private Rigidbody2D _rigidbody2D;
    private FixedJoint2D _fixedJoint2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _fixedJoint2D = GetComponent<FixedJoint2D>();
    }

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
            if (_fixedJoint2D != null)
            {
                Destroy(_fixedJoint2D);
            }

            _rigidbody2D.isKinematic = false;
            _rigidbody2D.velocity = new Vector2(0, -_fallSpeed);

            Debug.Log("Bubble destroyed because it is not attached to others.");
        }
    }

    private bool IsConnectedToOtherBubbles()
    {
        Collider2D[] nearbyBubbles = Physics2D
            .OverlapCircleAll(transform.position, _proximityRadius, _bubbleLayer);
        return nearbyBubbles.Length > 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos
            .DrawWireSphere(transform.position, _proximityRadius);
    }
}
