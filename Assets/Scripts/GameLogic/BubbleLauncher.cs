using UnityEngine;
using UnityEngine.Events;

public class BubbleLauncher : MonoBehaviour
{
    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _maxPullDistance = 2f;
    [SerializeField] private float _launchForceMultiplier = 10f;

    private Bubble _currentBubble;
    private bool _isDragging = false;
    private Camera _mainCamera;

    public UnityEvent OnBubbleLaunched;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Initialize(BubbleSpawner spawner)
    {
        spawner.OnBubbleSpawned.AddListener(AttachBubble);
    }

    public void AttachBubble(Bubble bubble)
    {
        _currentBubble = bubble;
        _currentBubble.transform.position = _launchPoint.position;

        var bubbleRb = _currentBubble.GetComponent<Rigidbody2D>();
        bubbleRb.isKinematic = true; 
    }

    private void Update()
    {
        if (_currentBubble == null) return; 
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !_isDragging)
        {
            Vector2 touchPosition = GetTouchPosition();
            if (IsTouchWithinLaunchArea(touchPosition))
            {
                _isDragging = true;
            }
        }

        if (_isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPosition = GetTouchPosition();
                DragBubble(touchPosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                ReleaseBubble();
            }
        }
    }

    private Vector2 GetTouchPosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsTouchWithinLaunchArea(Vector2 touchPosition)
    {
        return Vector2.Distance(touchPosition, _launchPoint.position) < _maxPullDistance;
    }

    private void DragBubble(Vector2 touchPosition)
    {
        Vector2 direction = touchPosition - (Vector2)_launchPoint.position;
        float distance = Mathf.Clamp(direction.magnitude, 0, _maxPullDistance);
        _currentBubble.transform.position = _launchPoint.position + (Vector3)(direction.normalized * distance);
    }

    private void ReleaseBubble()
    {
        _isDragging = false;

        Vector2 launchDirection = (_launchPoint.position - _currentBubble.transform.position).normalized;
        float pullDistance = Vector2.Distance(_currentBubble.transform.position, _launchPoint.position);

        Rigidbody2D bubbleRb = _currentBubble.GetComponent<Rigidbody2D>();
        bubbleRb.isKinematic = false; 
        bubbleRb.AddForce(_launchForceMultiplier * pullDistance * launchDirection, ForceMode2D.Impulse);

        if (_currentBubble.TryGetComponent<FixedJoint2D>(out var joint))
        {
            joint.enabled = true;
        }

        OnBubbleLaunched?.Invoke();
        _currentBubble = null;
    }
}
