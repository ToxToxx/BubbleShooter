using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;
using System;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private BubbleColors _bubbleColors;
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private int _maxBubbles = 10;
    [SerializeField] private int _poolIncrement = 5;
    [SerializeField] private BubbleLauncher _bubbleLauncher;
    [SerializeField] private float _spawnDelay = 1f;
    [SerializeField] private Transform _nextBubblePosition;
    [SerializeField] private SpriteRenderer _nextBubblePreview;

    public UnityEvent<Bubble> OnBubbleSpawned;
    public static Action OnBubbleHadSpawned;

    private IObjectPool<Bubble> _bubblePool;
    private IBubbleFactory _bubbleFactory;
    private Transform _transform;

    private int _activeBubbles = 0;
    private int _nextBubbleIndex = 0; 

    private void OnEnable()
    {
        _bubbleLauncher.OnBubbleLaunched.AddListener(HandleBubbleLaunched);
    }

    private void OnDisable()
    {
        _bubbleLauncher.OnBubbleLaunched.RemoveListener(HandleBubbleLaunched);
    }

    private void Awake()
    {
        _transform = transform;

        _bubbleFactory = new StandardBubbleFactory(_bubblePrefab);

        _bubblePool = new ObjectPool<Bubble>(
            _bubbleFactory.CreateBubble,
            OnGetBubble,
            OnReleaseBubble,
            OnDestroyBubble,
            maxSize: _maxBubbles
        );
    }

    private void Start()
    {
        PrepareNextBubble();  
        SpawnBubble();        
    }

    private void OnGetBubble(Bubble bubble)
    {
        bubble.Initialize(_bubbleColors.Color[_nextBubbleIndex], _bubbleColors.ColorId[_nextBubbleIndex]);
        bubble.gameObject.SetActive(true);

        _activeBubbles++;

        OnBubbleSpawned?.Invoke(bubble);
        OnBubbleHadSpawned?.Invoke();

        PrepareNextBubble();
    }

    private void PrepareNextBubble()
    {
        _nextBubbleIndex = UnityEngine.Random.Range(0, _bubbleColors.Color.Length);
        _nextBubblePreview.color = _bubbleColors.Color[_nextBubbleIndex];
    }

    private void OnReleaseBubble(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
        _activeBubbles--;
    }

    private void OnDestroyBubble(Bubble bubble)
    {
        Destroy(bubble.gameObject);
    }

    public void SpawnBubble()
    {
        EnsurePoolSize();
        Bubble bubble = _bubblePool.Get();
        bubble.transform.position = _transform.position;
    }

    public void ReleaseBubble(Bubble bubble)
    {
        _bubblePool.Release(bubble);
    }

    private void HandleBubbleLaunched()
    {
        StartCoroutine(SpawnBubbleAfterDelay(_spawnDelay));
    }

    private System.Collections.IEnumerator SpawnBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBubble();
    }

    private void EnsurePoolSize()
    {
        if (_activeBubbles >= _maxBubbles)
        {
            Debug.LogWarning("Max bubbles reached, increasing pool size.");
            _maxBubbles += _poolIncrement;

            _bubblePool = new ObjectPool<Bubble>(
                _bubbleFactory.CreateBubble,
                OnGetBubble,
                OnReleaseBubble,
                OnDestroyBubble,
                maxSize: _maxBubbles
            );
        }
    }
}
