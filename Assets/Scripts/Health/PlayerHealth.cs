using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthScript
{
    private Queue<Shield> shields;
    protected void Awake()
    {
        shields = new Queue<Shield>();
    }

    public void AddBuffer(Shield shield)
    {
        shields.Enqueue(shield);
    }
    
    public override bool ApplyDamage(float damage, Color dmgColor = new Color())
    {
        damage *= damageMultiplier;
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);
        if (dmgColor == new Color())
            dmgColor = this.damageColor;
        DamageTextManager.SpawnDamage(damage, worldPositionText, dmgColor);
        if (damage <= 0)
            return false;
        
        if (shields.Count > 0)
        {
            var shield = shields.Dequeue();
            bool isDestroyed = shield.Damage(damage);
            if (!isDestroyed)
            {
                shields.Enqueue(shield);
            }

            return false;
        }
        currentHealth -= damage;    
        PlayHurtSound(damage);
        EffectManager.Instance.PlayerHurtEffect(transform.position + Vector3.down, damage / currentHealth);
        if (currentHealth <= 0.0f)
        {
            GetComponent<PlayerDeathSequence>().StartDeathSequence();
            Destroy(this);
        }

        return true;
    }
    

    public void IncreaseCurrHealth(float healthIncrease)
    {
        DamageTextManager.SpawnDamage(healthIncrease, transform.position, Color.green);
        currentHealth += healthIncrease;
    }
    public void IncreaseMaxHealth(float healthIncrease)
    {
        maxHealth += healthIncrease;
    }
}