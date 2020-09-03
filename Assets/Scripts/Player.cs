using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public SkillCooldown skill1;
    public SkillCooldown skill2;
    public SkillCooldown skill3;

    public KeyCode keyCodeSkill1 = KeyCode.Q;
    public KeyCode keyCodeSkill2 = KeyCode.E;
    public KeyCode keyCodeSkill3 = KeyCode.R;

    public float skillCooldownTime1 = 1.4f;
    public float skillCooldownTime2 = 1.5f;
    public float skillCooldownTime3 = 1.6f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        SetSkillCooldown(skill1, skillCooldownTime1);
        SetSkillCooldown(skill2, skillCooldownTime2);
        SetSkillCooldown(skill3, skillCooldownTime3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        if (skill1.isCooldown == true)
        {
            skill1.UpdateSlider();
        } else
        {
            if (Input.GetKey(keyCodeSkill1)) {
                skill1.isCooldown = true;
            }
        }

        if (skill2.isCooldown == true)
        {
            skill2.UpdateSlider();
        }
        else
        {
            if (Input.GetKey(keyCodeSkill2))
            {
                skill2.isCooldown = true;
            }
        }

        if (skill3.isCooldown == true)
        {
            skill3.UpdateSlider();
        }
        else
        {
            if (Input.GetKey(keyCodeSkill3))
            {
                skill3.isCooldown = true;
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void SetSkillCooldown(SkillCooldown skill,float time)
    {
        skill.skillCooldown = time;
        skill.RestartSkill();
    }
}
