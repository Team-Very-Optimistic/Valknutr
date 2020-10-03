using System;
using UnityEngine;

public class Shield : NoTrigger
{
    private Transform parent;
    private float angularSpeed = 100f;
    private Quaternion rotation;
    private Vector3 orgPosition;
    private Transform newParent;
    private PlayerHealth playerHealth;
    private HealthScript _healthScript;
    private void Start()
    {
        base.Start();
        parent = transform.parent;
        playerHealth = parent.GetComponent<PlayerHealth>();
        orgPosition = parent.position - transform.position;
        newParent = new GameObject("shield").transform;
        newParent.transform.position = parent.position;
        transform.SetParent(newParent);
        _healthScript = gameObject.AddComponent<HealthScript>();
        _healthScript.maxHealth = 10f;
        _healthScript.hurtSound = "shieldHit";
        playerHealth = parent.GetComponent<PlayerHealth>();
        playerHealth.AddBuffer(this);
        AudioManager.PlaySoundAtPosition("shieldBuff", transform.position);
    }

    private void Update()
    {
        newParent.position = parent.position;   
        transform.RotateAround(parent.position, Vector3.up, angularSpeed * Time.deltaTime);
    }
    
    public void SetSpeed(float speed)
    {
        angularSpeed = speed;
    }

    public bool Damage(float damage)
    {
        return _healthScript.ApplyDamage(damage);
    }
}