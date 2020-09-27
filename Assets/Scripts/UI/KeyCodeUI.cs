using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeUI : MonoBehaviour
{
    public Sprite mouse1;
    public Sprite mouse2;
    public Sprite key;
    public TextMeshProUGUI text;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetKeyCode(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Clear)
        {
            image.enabled = false;
            text.text = "";
            return;
        }
        image.enabled = true;

        if (keyCode == KeyCode.Mouse0)
        {
            image.sprite = mouse1;
            text.text = "";
        }
        else if (keyCode == KeyCode.Mouse1)
        {
            image.sprite = mouse2;
            text.text = "";
        }
        else if (keyCode == KeyCode.Q)
        {
            image.sprite = key;
            text.text = "Q";

        }
        else if (keyCode == KeyCode.E)
        {
            image.sprite = key;
            text.text = "E";

        }
    }
}
