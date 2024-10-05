using UnityEngine;
using UnityEngine.Pool;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private BubbleColors _bubbleColors;
    [SerializeField] private GameObject _bubblePrefab;

    [SerializeField] private int _maxBubbles;
    private IObjectPool<Bubble> _bubblePool;

    private void Awake()
    {
        _bubblePool = new ObjectPool<Bubble>(
            CreateBubble,
            OnGetBubble,
            OnReleaseBubble,
            OnDestroyBubble,
            maxSize: _maxBubbles
        );
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
}
