using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{

    public Slider slider;

    public bool isCooldown;

    public float skillCooldown { get; set; }

    public void RestartSkill()
    {
        slider.maxValue = skillCooldown;
        slider.value = skillCooldown;
        isCooldown = false;
    }

    public void UpdateSlider()
    {
        float sliderChange = Time.deltaTime;
        slider.value -= sliderChange;
        if (slider.value <= 0)
        {
            RestartSkill();
        }
    }

}
