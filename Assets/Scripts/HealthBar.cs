using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI text;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health, float max)
    {
        slider.value = health;
        text.text = health.ToString() + "/" + max.ToString();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
