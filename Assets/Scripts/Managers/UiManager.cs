using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class UiManager : Singleton<UiManager>
{
    [HideInInspector]
    public GameObject player;
    public SpellDisplayScript[] spellSlots;
    public HealthBar healthBar;
    
    private HealthScript playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameManager.Instance._player;
        }
        PopulateSpells();


        playerHealth = player.GetComponent<HealthScript>();
        
        healthBar.SetMaxHealth(playerHealth.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null)
            healthBar.SetHealth(playerHealth.currentHealth);
    }

    void PopulateSpells()
    {
        var spells = player.GetComponent<SpellCaster>().spells;
        for (var i = 0; i < spells.Length && i < spellSlots.Length; i++)
        {
            spellSlots[i].SetSpell(spells[i]);
        }
    }
}
