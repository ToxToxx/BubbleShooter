using UnityEngine;
using UnityEngine.Events;

public class BubbleLauncher : MonoBehaviour
{
    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _maxPullDistance = 2f;
    [SerializeField] private float _launchForceMultiplier = 10f;
    [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
    [SerializeField] private float _scatterFactor = 6f;

    private Bubble _currentBubble;
    private Rigidbody2D _currentBubbleRb;  
    private bool _isDragging = false;
    private Camera _mainCamera;
    private float _pullDistance;

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
        _currentBubbleRb = _currentBubble.GetComponent<Rigidbody2D>();  
        _currentBubble.transform.position = _launchPoint.position;
        _currentBubbleRb.isKinematic = true;
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
            StartDragging();
        }

        if (_isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                ContinueDragging();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ReleaseBubble();
            }
        }
    }

    private void StartDragging()
    {
        Vector2 touchPosition = GetTouchPosition();
        if (IsTouchWithinLaunchArea(touchPosition))
        {
            _isDragging = true;
        }
    }

    private void ContinueDragging()
    {
        Vector2 touchPosition = GetTouchPosition();
        DragBubble(touchPosition);
        DrawTrajectoryPreview();
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
        _pullDistance = Mathf.Clamp(direction.magnitude, 0, _maxPullDistance);
        _currentBubble.transform.position = _launchPoint.position + (Vector3)(direction.normalized * _pullDistance);
    }

    private void ReleaseBubble()
    {
        _isDragging = false;

        Vector2 launchDirection = (_launchPoint.position - _currentBubble.transform.position).normalized;
        _currentBubbleRb.isKinematic = false;

        if (_pullDistance >= _maxPullDistance)
        {
            _currentBubble.gameObject.AddComponent<PiercingBubble>();  

            ApplyScatterEffect(_currentBubbleRb, launchDirection); 
        }
        else
        {
            _currentBubbleRb.AddForce(_launchForceMultiplier * _pullDistance * launchDirection, ForceMode2D.Impulse);
        }

        OnBubbleLaunched?.Invoke();  
        _currentBubble = null;
    }

    private void ApplyScatterEffect(Rigidbody2D bubbleRb, Vector2 launchDirection)
    {
        Vector2 scatterDirection1 = Quaternion.Euler(0, 0, _scatterFactor) * launchDirection;
        Vector2 scatterDirection2 = Quaternion.Euler(0, 0, -_scatterFactor) * launchDirection;

        bubbleRb.AddForce(_launchForceMultiplier * _pullDistance * (scatterDirection1 + scatterDirection2), ForceMode2D.Impulse);

        Debug.Log("Scatter effect applied: two forces with angle divergence.");
    }

    private void DrawTrajectoryPreview()
    {
        Vector2 dragDirection = _launchPoint.position - _currentBubble.transform.position;
        float pullDistance = Vector2.Distance(_currentBubble.transform.position, _launchPoint.position);

        Vector2 initialVelocity = _launchForceMultiplier * pullDistance * dragDirection.normalized;

        if (_pullDistance >= _maxPullDistance)
        {
            _trajectoryDrawer.DrawSplitTrajectory(_launchPoint.position, initialVelocity, _scatterFactor);
        }
        else
        {
            _trajectoryDrawer.DrawTrajectory(_launchPoint.position, initialVelocity);
        }
    }
}
