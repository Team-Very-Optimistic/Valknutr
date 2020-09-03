using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private bool isFriendly;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void DealDamage(Collider other)
    {
        if (other.gameObject.GetComponent<HealthScript>() != null)
        {
            other.gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
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
