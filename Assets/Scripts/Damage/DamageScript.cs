using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool isFriendly;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // If friendly projectile, check for "Enemies" tag
        // Else, it is enemy's projectile, check for "Player" tag

        if (isFriendly)
        {
            if(other.gameObject.CompareTag("Enemies"))
            {
                other.gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
        else
        {
            if(other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }

    //Getters/Setters
    public float GetDamage()
    {
        return damage;
    }    

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetIsFriendly(bool isFriendly)
    {
        this.isFriendly = isFriendly;
    }
}
