using UnityEngine;

public class PlayerHealth : HealthScript
{
    public override void ApplyDamage(float damage)
    {
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, height, 0.0f);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, worldPositionText, damageColor);
        if (damage <= 0)
            return;
        PlayHurtSound(damage);
        EffectManager.Instance.PlayerHurtEffect(transform.position, damage);
        
        currentHealth -= damage;    

        if (currentHealth <= 0.0f)
        {
            GetComponent<PlayerDeathSequence>().StartDeathSequence();
            Destroy(this);
        }
    }
    
}