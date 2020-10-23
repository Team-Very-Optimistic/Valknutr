using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Summon : SpellBehaviour
{
    private float _speed;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking;
    private Camera mainCam;
    private Damage _damageScript;
    private float offsetTime = .5f;
    private Vector3 offset;
    
    public override void SetProperties(float damage, float scale, float speed, float cooldown, params float[] additionalProperties)
    {
        var duration = additionalProperties[0];
        Destroy(gameObject, duration);
        _speed = speed;
        transform.localScale *= scale;
        _damageScript = gameObject.AddComponent<Damage>();
        _damageScript.SetDamage(damage);
    }

    public void Start()
    {
        base.Start();
        offset = new Vector3();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = _speed;
        navMeshAgent.angularSpeed += _speed;
        navMeshAgent.acceleration = _speed;
        mainCam = Camera.main;
        StartCoroutine(ChangeOffset(offsetTime));
    }

    IEnumerator ChangeOffset(float offsetTime)
    {
        yield return new WaitForSeconds(offsetTime);
        offset = Random.insideUnitSphere;
        offset.y = 0;
        StartCoroutine(ChangeOffset(offsetTime));
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
        navMeshAgent.SetDestination(target + offset);

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