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

        if (_bubbleCollider.sharedMaterial == null)
        {
            _bubbleCollider.sharedMaterial = new PhysicsMaterial2D();
        }

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
        return (layerMask.value & (1 << obj.layer)) > 0;
    }

    private void SetBounciness(float bounciness)
    {
        _bubbleCollider.sharedMaterial.bounciness = bounciness;

        _bubbleCollider.enabled = false; 
        _bubbleCollider.enabled = true; 
    }
}
