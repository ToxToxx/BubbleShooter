using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField] private int resolution = 30; // Number of points on the line
    [SerializeField] private LayerMask collisionMask; // Layer to check for collisions

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0; // Start with no points
    }

    public void DrawTrajectory(Vector2 startPosition, Vector2 initialVelocity)
    {
        _lineRenderer.positionCount = resolution;
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < resolution; i++)
        {
            Vector2 newPosition = PredictPosition(currentPosition, initialVelocity, i * Time.fixedDeltaTime);
            _lineRenderer.SetPosition(i, newPosition);
            currentPosition = newPosition;
        }
    }

    public void DrawSplitTrajectory(Vector2 startPosition, Vector2 initialVelocity, float scatterFactor)
    {
        _lineRenderer.positionCount = resolution * 2; // Twice the points for the two trajectories

        // Calculate two split trajectories
        Vector2 splitVelocity1 = Quaternion.Euler(0, 0, scatterFactor) * initialVelocity;
        Vector2 splitVelocity2 = Quaternion.Euler(0, 0, -scatterFactor) * initialVelocity;

        DrawSingleTrajectory(startPosition, splitVelocity1, 0); // Draw first trajectory
        DrawSingleTrajectory(startPosition, splitVelocity2, resolution); // Draw second trajectory
    }

    private void DrawSingleTrajectory(Vector2 startPosition, Vector2 initialVelocity, int startIndex)
    {
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < resolution; i++)
        {
            Vector2 newPosition = PredictPosition(currentPosition, initialVelocity, i * Time.fixedDeltaTime);
            _lineRenderer.SetPosition(startIndex + i, newPosition);
            currentPosition = newPosition;
        }
    }

    private Vector2 PredictPosition(Vector2 currentPosition, Vector2 velocity, float time)
    {
        return currentPosition + velocity * time + (time * time) * 0.5f * Physics2D.gravity; // Basic projectile motion
    }

    public void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0; // Clear the line
    }
}
