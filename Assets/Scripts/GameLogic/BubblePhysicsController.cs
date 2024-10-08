using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BubblePhysicsController : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _bubbleLayer;
    [SerializeField] private PhysicsMaterial2D _bubbleMaterial;

    private Collider2D _bubbleCollider;
    private bool _bouncinessDisabled = false;

    private void Awake()
    {
        _bubbleCollider = GetComponent<Collider2D>();

        if (_bubbleMaterial == null)
        {
            Debug.LogWarning("PhysicsMaterial2D is not assigned. Creating a default material.");
            _bubbleMaterial = new PhysicsMaterial2D("BubbleMaterial") { bounciness = 0.5f };
        }

        _bubbleCollider.sharedMaterial = _bubbleMaterial;

        SetBounciness(0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject, _wallLayer) && !_bouncinessDisabled)
        {
            Debug.Log("Bounced off the wall, bounciness retained at 0.5");
        }

        if (IsInLayerMask(collision.gameObject, _bubbleLayer))
        {
            if (!_bouncinessDisabled)
            {
                SetBounciness(0f);
                _bouncinessDisabled = true;
                Debug.Log("Hit another bubble, bounciness set to 0 and disabled.");
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }

    private void SetBounciness(float bounciness)
    {
        if (_bubbleCollider.sharedMaterial.bounciness != bounciness)
        {
            _bubbleCollider.sharedMaterial.bounciness = bounciness;

            _bubbleCollider.enabled = false;
            _bubbleCollider.enabled = true;
        }
    }
}
