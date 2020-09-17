using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private float currentHealth = 10;
    private GameObject damageTextPrefab;

    void Start()
    {
        damageTextPrefab = DamageTextManager.Instance.damageTextPrefab;
    }

    void Update()
    {

    }

    public void ApplyDamage(float damage)
    {
        Quaternion rotation = Quaternion.LookRotation(this.transform.position - GameObject.FindGameObjectWithTag("MainCamera").transform.position);
        GameObject damageText = Instantiate(damageTextPrefab);
        damageText.GetComponent<DamageText>().SetDamageTextProperties(damage, rotation, this.gameObject);

        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            // Perform death animation here
            Destroy(gameObject);
            SceneManager.LoadScene("GameOverScreen");
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
