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
  
    [HideInInspector]
    public GameObject damageTextPrefab;

    public Color damageColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    protected float height;
    public virtual void Start()
    {
        currentHealth = maxHealth;
        damageTextPrefab = DamageTextManager.Instance.damageTextPrefab;
        
        height = GetComponent<Collider>().bounds.size.y / 2.0f;
    }

    public virtual bool ApplyDamage(float damage)
    {
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, worldPositionText, damageColor);
        if (damage <= 0)
            return false;
        if (hurtSoundOnHit)
        {
            PlayHurtSound(damage);
        }
        
        currentHealth -= damage;
        if (currentHealth > 0.0f) return false;

        //todo: Derive from enemy instead
        if (gameObject.tag == "Enemy")
        {
            GetComponent<EnemyDeathSequence>().StartDeathSequence();
        }
        else
        {
            Destroy(gameObject);
        }
        return true;
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
        currentHealth = health;
    }

    public void AddHealth(float health)
    {
        currentHealth += health;
    }
}