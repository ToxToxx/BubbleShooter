using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField] private int _resolution = 30; 
    [SerializeField] private LayerMask _collisionMask; 

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0; 
    }

    public void DrawTrajectory(Vector2 startPosition, Vector2 initialVelocity)
    {
        _lineRenderer.positionCount = _resolution;
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < _resolution; i++)
        {
            Vector2 newPosition = PredictPosition(currentPosition, initialVelocity, i * Time.fixedDeltaTime);
            _lineRenderer.SetPosition(i, newPosition);
            currentPosition = newPosition;
        }
    }

    public void DrawSplitTrajectory(Vector2 startPosition, Vector2 initialVelocity, float scatterFactor)
    {
        _lineRenderer.positionCount = _resolution * 2; 

        Vector2 splitVelocity1 = Quaternion.Euler(0, 0, scatterFactor) * initialVelocity;
        Vector2 splitVelocity2 = Quaternion.Euler(0, 0, -scatterFactor) * initialVelocity;

        DrawSingleTrajectory(startPosition, splitVelocity1, 0); 
        DrawSingleTrajectory(startPosition, splitVelocity2, _resolution); 
    }

    private void DrawSingleTrajectory(Vector2 startPosition, Vector2 initialVelocity, int startIndex)
    {
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < _resolution; i++)
        {
            Vector2 newPosition = PredictPosition(currentPosition, initialVelocity, i * Time.fixedDeltaTime);
            _lineRenderer.SetPosition(startIndex + i, newPosition);
            currentPosition = newPosition;
        }
    }

    private Vector2 PredictPosition(Vector2 currentPosition, Vector2 velocity, float time)
    {
        return currentPosition + velocity * time + (time * time) * 0.5f * Physics2D.gravity; 
    }

    public void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }
}
