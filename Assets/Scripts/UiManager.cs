using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class UiManager : Singleton<UiManager>
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public SkillCooldown skill1;
    public SkillCooldown skill2;
    public SkillCooldown skill3;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        skill1.RestartSkill();
        skill2.RestartSkill();
        skill3.RestartSkill();
    }

    // Update is called once per frame
    void Update()
    {
        // if (skill1.isCooldown == true)
        // {
        //     skill1.UpdateSlider();
        // } 
        //
        // if (skill2.isCooldown == true)
        // {
        //     skill2.UpdateSlider();
        // }
        //
        // if (skill3.isCooldown == true)
        // {
        //     skill3.UpdateSlider();
        // }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void SetSkillCooldown(int index, float time)
    {
        switch (index)
        {
            case 1:
                skill1.skillCooldown = time;
                break;
            case 2:
                skill2.skillCooldown = time;
                break;
            case 3:
                skill3.skillCooldown = time;
                break;
        }
    }
}
