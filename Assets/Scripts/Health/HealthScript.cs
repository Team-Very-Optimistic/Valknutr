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

    void Start()
    {
        damageTextPrefab = DamageTextManager.Instance.damageTextPrefab;
    }

    public void ApplyDamage(float damage)
    {
        Quaternion rotation = Quaternion.LookRotation(this.transform.position - GameObject.FindGameObjectWithTag("MainCamera").transform.position);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, rotation, this.gameObject);

        if (hurtSoundOnHit)
        {
            PlayHurtSound(damage);
        }
        

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            // Perform death animation here
            if (destroyOnDeath)
                Destroy(gameObject);
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
