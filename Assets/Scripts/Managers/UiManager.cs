using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class UiManager : Singleton<UiManager>
{
    public HealthBar healthBar;

    public SkillCooldown skill1;
    public SkillCooldown skill2;
    public SkillCooldown skill3;
    
    public GameObject player;
    private HealthScript playerHealth;
    

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameManager.Instance._player;
        }

        playerHealth = player.GetComponent<HealthScript>();
        
        healthBar.SetMaxHealth(playerHealth.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null)
            healthBar.SetHealth(playerHealth.currentHealth);
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
