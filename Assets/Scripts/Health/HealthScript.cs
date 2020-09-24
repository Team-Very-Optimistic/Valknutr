using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth = 10;
    public bool destroyOnDeath = true;
    public string hurtSound;
    public bool hurtSoundOnHit = true;

    private GameObject damageTextPrefab;

    public Color damageColor;

    void Start()
    {
        damageTextPrefab = DamageTextManager.Instance.damageTextPrefab;
    }

    public void ApplyDamage(float damage)
    {
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, this.GetComponent<CapsuleCollider>().height / 2.0f, 0.0f);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, worldPositionText, damageColor);

        if (hurtSoundOnHit)
        {
            PlayHurtSound(damage);
        }
        

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            if(gameObject.tag == "Enemy")
            {
                this.GetComponent<EnemyDeathSequence>().StartDeathSequence();
            }
        }
    }

    private void PlayHurtSound(float damage)
    {
        var percent = damage / maxHealth;
        var volume = Mathf.Sqrt(percent) * 1.8f + 0.2f;
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
