using TMPro;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI titleTextObject;
    public TextMeshProUGUI bodyTextObject;

    public void Show(string titleText, string bodyText)
    {
        gameObject.SetActive(true);
        titleTextObject.text = titleText;
        bodyTextObject.text = bodyText;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Tooltip tooltip)
    {
        Show(tooltip.Title, tooltip.Body);
    }
}