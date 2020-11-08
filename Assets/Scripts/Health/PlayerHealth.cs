using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthScript
{
    private Queue<Shield> shields;

    public event GameManager.PlayerDeathAction OnPlayerDeath;
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
            OnPlayerDeath?.Invoke();
            GetComponent<PlayerDeathSequence>().StartDeathSequence();
            Destroy(this);
        }

        return true;
    }

    public IEnumerator ApplyDamageOverTime(float damagePerTick, float numTicks, float totalDuration, Color damageColor)
    {
        damagePerTick *= damageMultiplier;

        Debug.Log(this.gameObject);
        Debug.Log("called coroutine");
        //Ticks starts after timeInterval and ends on last frame(?)
        float timeInterval = totalDuration / (float)numTicks;
        Debug.Log(timeInterval);

        for (int i = 0; i < numTicks; i++)
        {
            Debug.Log("before wait for tick " + i);
            yield return new WaitForSeconds(timeInterval);
            Debug.Log("after wait for tick " + i);

            Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);
            DamageTextManager.SpawnDamage(damagePerTick, worldPositionText, damageColor);

            if (shields.Count > 0)
            {
                var shield = shields.Dequeue();
                bool isDestroyed = shield.Damage(damagePerTick);
                if (!isDestroyed)
                {
                    shields.Enqueue(shield);
                }
            }
            else
            {
                currentHealth -= damagePerTick;
                PlayHurtSound(damagePerTick);
                EffectManager.Instance.PlayerHurtEffect(transform.position + Vector3.down, damagePerTick / currentHealth);
                if (currentHealth <= 0.0f)
                {
                    OnPlayerDeath?.Invoke();
                    GetComponent<PlayerDeathSequence>().StartDeathSequence();
                    Destroy(this);
                }
            }
        }
    }

    public void IncreaseCurrHealth(float healthIncrease)
    {
        if (currentHealth + healthIncrease >= maxHealth)
        {
            healthIncrease = maxHealth - currentHealth;
            if (currentHealth == 0)
            {
                return;
                
            }
        }
        DamageTextManager.SpawnDamage(healthIncrease, transform.position, new Color(1, 0.6f, 0.9f));
        currentHealth += healthIncrease;
    }
    public void IncreaseMaxHealth(float healthIncrease)
    {
        maxHealth += healthIncrease;
    }
}