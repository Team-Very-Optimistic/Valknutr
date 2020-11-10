using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 10;
    [HideInInspector]
    public float currentHealth = 10;
    [HideInInspector]
    public bool destroyOnDeath = true;
    public string hurtSound;
    public bool hurtSoundOnHit = true;
    public bool dead = false;
    
    //Damage protection multipliers
    protected float damageMultiplier = 1.0f;
    private bool hasShield = false;

    public Color damageColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color damageWithShieldColor;

    protected float height;
    private float invulnTime;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        var c =GetComponent<Collider>();
        if(c)
            height = c.bounds.size.y / 2.0f;
        else
        {
            height = 4f;
        }
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }
    private void Update()
    {
        invulnTime = Mathf.Clamp(invulnTime - Time.deltaTime, 0, Single.MaxValue);
    }

    public virtual bool ApplyDamage(float damage, Color dmgColor = new Color())
    {
        if (IsInvulnerable()) return false;
        float finalDamage = damage * damageMultiplier;
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);

        if(hasShield)
        {
            DamageTextManager.SpawnDamage(finalDamage, worldPositionText, damageWithShieldColor);
        }
        else
        {
            if (dmgColor == new Color())
                dmgColor = this.damageColor;

            DamageTextManager.SpawnDamage(finalDamage, worldPositionText, dmgColor);
        }
       
        EffectManager.Instance.EnemyHurtEffect();

        if (damage <= 0)
            return false;

        if (hurtSoundOnHit)
        {
            PlayHurtSound(finalDamage);
        }
        
        currentHealth -= finalDamage;
        if (currentHealth > 0.0f || dead) return false;

        dead = true;

        //todo: Derive from enemy instead
        if (gameObject.tag == "Enemy")
        {
            GetComponent<EnemyDeathSequence>().StartDeathSequence();
        }
        else
        {
            OnDeath();
        }
        return true;
    }

    protected bool IsInvulnerable()
    {
        return invulnTime != 0;
    }

    public IEnumerator ApplyDamageOverTime(float damagePerTick, float numTicks, float totalDuration, Color damageColor)
    {
        float timeInterval = totalDuration / numTicks;
        
        for (int i = 0; i < numTicks; i++)
        {
            yield return new WaitForSeconds(timeInterval);
            ApplyDamage(damagePerTick, damageColor);
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected void PlayHurtSound(float damage)
    {
        var percent = damage / maxHealth;
        var volume = Mathf.Sqrt(percent);
        var pitch = Random.Range(0.8f, 1.2f);

        AudioManager.PlaySoundAtPosition(hurtSound, transform.position, volume, pitch);
    }

    //Getters/Setters
    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float health)
    {
        if (health > maxHealth)
        {
            maxHealth = health;
        }
        currentHealth = health;
    }

    public void AddHealth(float health)
    {
        currentHealth += health;
    }

    public void AdditivelyAddDmgMultiplier(float damageMultiplier)
    {
        // overlap multipliers
        this.damageMultiplier *= damageMultiplier;
    }
    

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1.0f;
    }

    public void SetHasShield(bool hasShield)
    {
        this.hasShield = hasShield;
    }

    public void SetShieldDamageColor(Color damageWithShieldColor)
    {
        this.damageWithShieldColor = damageWithShieldColor;
    }

    public void SetInvulnerable(float duration)
    {
        print("invulnerable");
        invulnTime = duration;
    }

    public void Scale(float healthMultiplier)
    {
        currentHealth *= healthMultiplier;
        maxHealth *= healthMultiplier;
    }
}