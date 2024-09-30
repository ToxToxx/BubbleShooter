using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D _ballRb;
    private bool _isShooting;

    private void Awake()
    {
        _ballRb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction, float force)
    {
        _ballRb.isKinematic = false;
        _ballRb.velocity = direction * force;
        _isShooting = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isShooting)
        {
            _ballRb.velocity = Vector2.zero;
            _ballRb.isKinematic = true;
            _isShooting = false;

            AttachToNeighboringBubbles();
            CheckForMatches();
        }
    }

    private void AttachToNeighboringBubbles()
    {
        Collider2D[] nearbyBubbles = Physics2D.OverlapCircleAll(transform.position, 1.1f);
        foreach (Collider2D bubble in nearbyBubbles)
        {
            if (bubble.gameObject != gameObject)
            {
                SpringJoint2D newJoint = gameObject.AddComponent<SpringJoint2D>();
                newJoint.connectedBody = bubble.GetComponent<Rigidbody2D>();
                newJoint.autoConfigureDistance = false;
                newJoint.distance = 0.5f;
                newJoint.frequency = 1f;
                newJoint.dampingRatio = 0.5f;
            }
        }
    }

    private void CheckForMatches()
    {
        Color myColor = GetComponent<Renderer>().material.color;
        Collider2D[] nearbyBubbles = Physics2D.OverlapCircleAll(transform.position, 1.1f);

        int matchCount = 0;
        foreach (Collider2D bubble in nearbyBubbles)
        {
            if (bubble.GetComponent<Renderer>().material.color == myColor)
            {
                matchCount++;
            }
        }

        if (matchCount >= 2)
        {
            Explode();
            foreach (Collider2D bubble in nearbyBubbles)
            {
                if (bubble.GetComponent<Renderer>().material.color == myColor)
                {
                    bubble.GetComponent<Ball>().Explode();
                }
            }
        }
    }

    public void Explode()
    {
        SpringJoint2D[] joints = GetComponents<SpringJoint2D>();
        foreach (SpringJoint2D joint in joints)
        {
            Destroy(joint);
        }
        StartCoroutine(ExplodeRoutine());
    }

    private IEnumerator ExplodeRoutine()
    {
        Vector3 initialScale = transform.localScale;
        float time = 0;
        while (time < 1)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, time);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
