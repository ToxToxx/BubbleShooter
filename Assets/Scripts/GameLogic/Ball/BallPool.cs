using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private int _poolSize = 20;

    private readonly Queue<GameObject> pool = new();

    private void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_ballPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBall(Vector3 position)
    {
        if (pool.Count > 0)
        {
            GameObject ball = pool.Dequeue();
            ball.SetActive(true);
            ball.transform.position = position;
            ball.GetComponent<Rigidbody2D>().isKinematic = true;
            return ball;
        }
        else
        {
            GameObject newBall = Instantiate(_ballPrefab);
            newBall.transform.position = position;
            return newBall;
        }
    }

    public void ReturnBall(GameObject ball)
    {
        ball.SetActive(false);
        pool.Enqueue(ball);
    }

    public List<GameObject> GetActiveBalls()
    {
        List<GameObject> activeBalls = new();
        foreach (GameObject ball in pool)
        {
            if (ball.activeSelf)
            {
                activeBalls.Add(ball);
            }
        }
        return activeBalls;
    }
}
