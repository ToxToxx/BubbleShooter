using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _trajectoryLine;
    [SerializeField] private BallPool _ballPool;
    private GameObject _activeBall;
    [SerializeField] private float _shootForce = 10f;
    private Vector2 _shootDirection;

    private void Start()
    {
        SpawnNewActiveBall(); 
    }

    private void Update()
    {
        if (_activeBall == null)
        {
            SpawnNewActiveBall(); 
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _shootDirection = (mousePosition - _activeBall.transform.position).normalized;
            DrawTrajectory();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _activeBall.GetComponent<Ball>().Shoot(_shootDirection, _shootForce);
            _activeBall = null; 
        }
    }

    private void DrawTrajectory()
    {
        _trajectoryLine.SetActive(true);
        LineRenderer lr = _trajectoryLine.GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, _activeBall.transform.position);
        lr.SetPosition(1, _activeBall.transform.position + (Vector3)_shootDirection * _shootForce);
    }


    private void SpawnNewActiveBall()
    {
        Vector3 spawnPosition = new(0, -4, 0);
        _activeBall = _ballPool.GetBall(spawnPosition); 
        _activeBall.GetComponent<Ball>().SetPool(_ballPool); 
    }

    public void CheckFloatingBubbles()
    {
        foreach (GameObject bubble in _ballPool.GetActiveBalls())
        {
            if (!IsBubbleConnected(bubble))
            {
                Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
                rb.isKinematic = false;
            }
        }
    }

    private bool IsBubbleConnected(GameObject bubble)
    {
        SpringJoint2D[] joints = bubble.GetComponents<SpringJoint2D>();
        return joints.Length > 0;
    }
}
