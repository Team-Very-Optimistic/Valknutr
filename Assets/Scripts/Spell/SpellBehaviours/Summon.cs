using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Summon : TriggerEventHandler
{
    private float _speed;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking;
    private Camera mainCam;
    private Damage _damageScript;

    public void Set(float duration, float speed, float damage, float scale)
    {
        Destroy(gameObject, duration);
        _speed = speed;
        transform.localScale *= scale;
        _damageScript = gameObject.AddComponent<Damage>();
        _damageScript.SetDamage(damage);

    }

    public void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = _speed;
        navMeshAgent.angularSpeed += _speed;
        navMeshAgent.acceleration = _speed;
        mainCam = Camera.main;
    }
    public void DamageEnemy(){}

    public override void TriggerEvent(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (_damageScript.DealDamage(other))
        {
            EffectManager.PlayEffectAtPosition("summonDmg", transform.position, transform.lossyScale / 2f);
            AudioManager.PlaySoundAtPosition("summonDmg", transform.position);
        }
    }
    
    public void Update()
    {
        //Animation
        var target = Util.GetMousePositionOnWorldPlane(mainCam);
        //Navigation
        navMeshAgent.SetDestination(target);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            if (navMeshAgent.isStopped) navMeshAgent.isStopped = false;
        }
        else
        {
            if (!navMeshAgent.isStopped) navMeshAgent.isStopped = true;

            transform.LookAt(target);
        }
    }
}