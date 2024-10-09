using UnityEngine;
using TMPro;

public class LinkText : MonoBehaviour
{
    [SerializeField] private TMP_Text _linkText;
    [SerializeField] private string url = "https://vk.com/rachelovecoffee";

    private void Start()
    {
        _linkText.text = $"<link=\"{url}\">Authors VK</link>";
        _linkText.richText = true;
    }

    private void Update()
    {
        HandleLinkClick();
    }

    private void HandleLinkClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_linkText, Input.mousePosition, Camera.main);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _linkText.textInfo.linkInfo[linkIndex];
                string selectedLink = linkInfo.GetLinkID();

                Debug.Log($"Opening URL: {selectedLink}");
                Application.OpenURL(selectedLink);
            }
        }
    }
}
