using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private BubbleColors _bubbleColors;
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private int _maxBubbles;
    [SerializeField] private BubbleLauncher _bubbleLauncher;

    public UnityEvent<Bubble> OnBubbleSpawned;

    private IObjectPool<Bubble> _bubblePool;

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
        _bubblePool = new ObjectPool<Bubble>(
            CreateBubble,
            OnGetBubble,
            OnReleaseBubble,
            OnDestroyBubble,
            maxSize: _maxBubbles
        );
    }

    private void Start()
    {
        SpawnBubble();
    }

    private Bubble CreateBubble()
    {
        GameObject bubbleInstance = Instantiate(_bubblePrefab);
        return bubbleInstance.GetComponent<Bubble>();
    }

    private void OnGetBubble(Bubble bubble)
    {
        int randomIndex = Random.Range(0, _bubbleColors.Color.Length);
        Color randomColor = _bubbleColors.Color[randomIndex];
        char randomColorId = _bubbleColors.ColorId[randomIndex];

        bubble.Initialize(randomColor, randomColorId);
        bubble.gameObject.SetActive(true);

        OnBubbleSpawned?.Invoke(bubble);
    }

    private void OnReleaseBubble(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
    }

    private void OnDestroyBubble(Bubble bubble)
    {
        Destroy(bubble.gameObject);
    }

    public void SpawnBubble()
    {
        Bubble bubble = _bubblePool.Get();
        bubble.transform.position = transform.position;
    }

    public void ReleaseBubble(Bubble bubble)
    {
        _bubblePool.Release(bubble);
    }

    private void HandleBubbleLaunched()
    {
        StartCoroutine(SpawnBubbleAfterDelay(1f)); 
    }

    private System.Collections.IEnumerator SpawnBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBubble();
    }
}
