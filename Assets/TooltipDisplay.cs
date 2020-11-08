using System;
using TMPro;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI titleTextObject;
    public TextMeshProUGUI bodyTextObject;
    public TextMeshProUGUI subtitleObject;

    private void Start()
    {
        titleTextObject.text = "";
        bodyTextObject.text = "";
    }

    public void Show(string titleText, string bodyText, string subtitleText="")
    {
        gameObject.SetActive(true);
        titleTextObject.text = titleText;
        bodyTextObject.text = bodyText;
        subtitleObject.text = subtitleText;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Tooltip tooltip)
    {
        Show(tooltip.Title, tooltip.Body, tooltip.Subtitle);
    }
}