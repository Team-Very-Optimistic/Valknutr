using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private float currentHealth = 10;
    private GameObject damageTextPrefab;

    public Color damageColor;

    void Start()
    {
        damageTextPrefab = DamageTextManager.Instance.damageTextPrefab;
    }

    void Update()
    {

    }

    public void ApplyDamage(float damage)
    {
        Vector3 worldPositionText = transform.position + new Vector3(0.0f, this.GetComponent<CapsuleCollider>().height / 2.0f, 0.0f);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, worldPositionText, damageColor);

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            // Perform death animation here
            Destroy(gameObject);
        }
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
