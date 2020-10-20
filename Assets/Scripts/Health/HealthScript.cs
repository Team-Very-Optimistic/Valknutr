using System.Collections;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 10;
    [HideInInspector]
    public float currentHealth = 10;
    [HideInInspector]
    public bool destroyOnDeath = true;
    public string hurtSound;
    public bool hurtSoundOnHit = true;
  


    //Damage protection multipliers
    private float damageMultiplier = 1.0f;

    public Color damageColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    protected float height;
    public virtual void Start()
    {
        currentHealth = maxHealth;
       
        
        height = GetComponent<Collider>().bounds.size.y / 2.0f;
    }

    public virtual bool ApplyDamage(float damage)
    {
        float finalDamage = damage * damageMultiplier;

        Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);
        DamageTextManager.SpawnDamage(finalDamage, worldPositionText, damageColor);
        EffectManager.Instance.EnemyHurtEffect();
        if (damage <= 0)
            return false;
        if (hurtSoundOnHit)
        {
            PlayHurtSound(finalDamage);
        }
        
        currentHealth -= finalDamage;
        if (currentHealth > 0.0f) return false;

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

    public void SetDamageMultiplier(float damageMultiplier)
    {
        //If not original value, dont overlap multipliers
        if(this.damageMultiplier == 1.0f)
        {
            this.damageMultiplier = damageMultiplier;
        }
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1.0f;
    }
}