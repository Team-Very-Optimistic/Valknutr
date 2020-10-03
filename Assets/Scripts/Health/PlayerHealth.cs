using System;
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
    
    public override bool ApplyDamage(float damage)
    {
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, worldPositionText, damageColor);
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
        EffectManager.Instance.PlayerHurtEffect(transform.position, damage);
        if (currentHealth <= 0.0f)
        {
            GetComponent<PlayerDeathSequence>().StartDeathSequence();
            Destroy(this);
        }

        return true;
    }
    
}