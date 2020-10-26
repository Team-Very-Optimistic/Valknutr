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
    private bool isPhase;
    
    public override void SetProperties(float damage, float scale, float speed, float cooldown, params float[] additionalProperties)
    {
        var duration = additionalProperties[0];
        Destroy(gameObject, duration);
        _speed = speed;
        transform.localScale *= scale;
        _damageScript = gameObject.AddComponent<Damage>();
        _damageScript.SetDamage(damage);
    }

    protected override void Start()
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
        isPhase = (bool) GetComponent<Phasing>();
        if(isPhase)
            navMeshAgent.enabled = false;
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
    private Vector3 currentVelocity = Vector3.zero;
    public void Update()
    {
        //Animation
        var target = Util.GetMousePositionOnWorldPlane(mainCam);
        target = target + offset;
        //Navigation
        navMeshAgent.SetDestination(target);

        if (isPhase)
        {
            target += Vector3.up;
            
            transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, 0.3f, _speed);;
        }
        
       

        var rotation = Quaternion.LookRotation (target - transform.position);
        // rotation.x = 0; This is for limiting the rotation to the y axis. I needed this for my project so just
        // rotation.z = 0;                 delete or add the lines you need to have it behave the way you want.
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * _speed/5f);
    }


}