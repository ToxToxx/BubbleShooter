using UnityEngine;

public class GameCanvasGroupLogic : MonoBehaviour
{
    [SerializeField] private CanvasGroup _winCanvasGroup;
    [SerializeField] private CanvasGroup _looseCanvasGroup;
    [SerializeField] private CanvasGroup _currentCanvasGroup;

    private void OnEnable()
    {
        LevelManager.OnGameWon += LevelManager_OnGameWon;
        GameOverLogic.OnGameLoose += GameOverLogic_OnGameLoose;
    }

    private void GameOverLogic_OnGameLoose(object sender, System.EventArgs e)
    {
        LooseCanvasGroupVisibilityToggle();
    }

    private void LevelManager_OnGameWon(object sender, System.EventArgs e)
    {
        WinCanvasGroupVisibilityToggle();
    }

    private void OnDisable()
    {
        LevelManager.OnGameWon -= LevelManager_OnGameWon;
        GameOverLogic.OnGameLoose -= GameOverLogic_OnGameLoose;
    }

    private void WinCanvasGroupVisibilityToggle()
    {
        _winCanvasGroup.alpha = 1f;
        _winCanvasGroup.interactable = true;
        _winCanvasGroup.blocksRaycasts = true;

        _currentCanvasGroup.alpha = 0f;
        _currentCanvasGroup.interactable = false;
        _currentCanvasGroup.blocksRaycasts = false;
    }

    private void LooseCanvasGroupVisibilityToggle()
    {
        _looseCanvasGroup.alpha = 1f;
        _looseCanvasGroup.interactable = true;
        _looseCanvasGroup.blocksRaycasts = true;

        _currentCanvasGroup.alpha = 0f;
        _currentCanvasGroup.interactable = false;
        _currentCanvasGroup.blocksRaycasts = false;
    }
}
