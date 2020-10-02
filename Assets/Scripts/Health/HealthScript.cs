using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 10;
    [HideInInspector]
    public float currentHealth = 10;
    [HideInInspector]
    public bool destroyOnDeath = true;
    public string hurtSound;
    public bool hurtSoundOnHit = true;
    [HideInInspector]
    public bool isPlayer;
    [HideInInspector]
    public GameObject damageTextPrefab;

    public Color damageColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public virtual void Start()
    {
        currentHealth = maxHealth;
        damageTextPrefab = DamageTextManager.Instance.damageTextPrefab;
        
        if (gameObject == GameManager.Instance._player)
        {
            isPlayer = true;
        }
    }

    public virtual void ApplyDamage(float damage)
    {
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, this.GetComponent<Collider>().bounds.size.y / 2.0f, 0.0f);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, worldPositionText, damageColor);
        if (damage <= 0)
            return;
        if (hurtSoundOnHit)
        {
            PlayHurtSound(damage);
        }

        if (isPlayer)
        {
            EffectManager.Instance.PlayerHurtEffect(transform.position, damage);
        }

        currentHealth -= damage;    

        if (currentHealth <= 0.0f)
        {
            if (gameObject.tag == "Enemy")
            {
                GetComponent<EnemyDeathSequence>().StartDeathSequence();
            }
            else if (isPlayer)
            {
                GetComponent<PlayerDeathSequence>().StartDeathSequence();
                Destroy(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void PlayHurtSound(float damage)
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
        currentHealth = health;
    }

    public void AddHealth(float health)
    {
        currentHealth += health;
    }
}