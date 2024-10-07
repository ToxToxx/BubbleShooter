using UnityEngine;

public class StartupManager : MonoBehaviour
{
    [SerializeField] private BubbleSpawner _bubbleSpawner;
    [SerializeField] private BubbleLauncher _bubbleLauncher;

    private void Start()
    {
        _bubbleLauncher.Initialize(_bubbleSpawner);
    }
}
