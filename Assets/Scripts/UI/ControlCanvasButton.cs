using UnityEngine;
using UnityEngine.UI;

public class ControlCanvasButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup _targetCanvasGroup;
    [SerializeField] private CanvasGroup _currentCanvasGroup;
    [SerializeField] private Button _controlCanvasButton;

    private void OnEnable()
    {
        if (_controlCanvasButton != null && _targetCanvasGroup != null && _currentCanvasGroup != null)
        {

            _controlCanvasButton.onClick.AddListener(ToggleCanvasVisibility);
        }
        else
        {
            Debug.LogError("CanvasGroup or Button is missing.");
        }
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

    private void OnDisable()
    {
        if (_controlCanvasButton != null)
        {
            _controlCanvasButton.onClick.RemoveListener(ToggleCanvasVisibility);
        }
    }
}
