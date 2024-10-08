using UnityEngine;

public class ControlCanvasGroupEvent : MonoBehaviour
{
    [SerializeField] private CanvasGroup _targetCanvasGroup;
    [SerializeField] private CanvasGroup _currentCanvasGroup;

    private void OnEnable()
    {
        LevelManager.OnGameWon += LevelManager_OnGameWon;
    }

    private void LevelManager_OnGameWon(object sender, System.EventArgs e)
    {
        ToggleCanvasVisibility();
    }

    private void OnDisable()
    {
        LevelManager.OnGameWon -= LevelManager_OnGameWon;
    }

    private void ToggleCanvasVisibility()
    {
        _targetCanvasGroup.alpha = 1f;
        _targetCanvasGroup.interactable = true;
        _targetCanvasGroup.blocksRaycasts = true;

        _currentCanvasGroup.alpha = 0f;
        _currentCanvasGroup.interactable = false;
        _currentCanvasGroup.blocksRaycasts = false;
    }
}
