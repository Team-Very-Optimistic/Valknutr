using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour_Spider : EnemyBehaviourBase
{
    enum SpiderBehaviourStates
    {
        Running,
        Windup,
        Jump,
        Explode //Not used
    }

    private SpiderBehaviourStates spiderState;

    public float windUpTime;

    //Jump
    private Vector3 originalPosition;
    private float jumpHeight;
    private float jumpTimeElapsed;
    [SerializeField] private AnimationCurve jumpCurve;
    private float lastKeyTime;

    //Explode
    public float explodeRadius = 4.0f;
    public float explodeDuration = 1.0f;
    public GameObject explodeParticlePrefab;

    //Post-explode lingering damage
    public GameObject poisonLingerParticlePrefab;
    public Color explodeDamageColor;
    [SerializeField]
    private float explodeDamagePerTick = 1.0f;
    [SerializeField]

    private float explodeDamageTotalDuration = 2.0f;
    [SerializeField]

    private int explodeDamageNumTicks = 2;

    public GameObject redIndicatorPrefab;
    private Vector3 redIndicatorPosOffset;

    //NavMeshAgent SetDestination bug: remaining distance always starts off at zero
    int wait = 0, waitTicks = 5;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        jumpHeight = player.GetComponent<Collider>().bounds.size.y;

        //Set running state
        spiderState = SpiderBehaviourStates.Running;

        lastKeyTime = jumpCurve[jumpCurve.length - 1].time;

        redIndicatorPosOffset = new Vector3(0.0f, GetComponent<Collider>().bounds.size.y, 0.0f);

        //Disable knockback (buggy with navmesh)
        canKnockback = false;

        ResetWaitTicks();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (navMeshAgent.enabled) // Not in knockback
        {
            switch (spiderState)
            {
                case SpiderBehaviourStates.Running:
                    {
                        //Navigation
                        navMeshAgent.SetDestination(player.transform.position);

                        if (--wait > 0) return;

                        //If close enough to player, switch to wind up
                        if (navMeshAgent.hasPath && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                        {
                            SetupJump();
                        }

                        break;
                    }
                case SpiderBehaviourStates.Windup:
                    {
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        break;
                    }
                case SpiderBehaviourStates.Jump:
                    {
                        jumpTimeElapsed += Time.deltaTime;
                        float yOffset = jumpCurve.Evaluate(jumpTimeElapsed) * jumpHeight;

                        transform.position = originalPosition + new Vector3(0.0f, yOffset, 0.0f);

                        if (jumpTimeElapsed >= lastKeyTime)
                        {
                            Explode();
                        }
                        break;
                    }
                case SpiderBehaviourStates.Explode:
                    {
                        break;
                    }
            }
        }
        else
        {
            //Check in knockback state before stopping knockback state - Velocity update not neccesarily within same frame of enableknockback
            if (!isInKnockback)
            {
                if (rigidbody.velocity.magnitude > 0.0f)
                {
                    isInKnockback = true;
                }
            }
            else
            {
                if (rigidbody.velocity.magnitude <= knockbackVelStoppingThreshold)
                {
                    EnableKnockback(false);
                    isInKnockback = false;
                }
            }
        }
    }

    private void SetupJump()
    {
        //Invoke show red indicator function + start charge function
        Invoke(nameof(ShowRedIndicator), windUpTime * 0.5f);
        Invoke(nameof(StartJump), windUpTime);
        Invoke(nameof(PlaySqueal), windUpTime * 0.5f);

        //Change enum state
        spiderState = SpiderBehaviourStates.Windup;

        //Trigger anim
        ResetAllAnimationTriggers();
        animator.SetTrigger("ToWindup");

        ResetWaitTicks();
    }

    private void PlaySqueal()
    {
        AudioManager.PlaySound("SpiderSqueal", 0.3f, Random.Range(0.75f, 1.25f));
    }

    private void ShowRedIndicator()
    {
        float timeToDestroy = windUpTime * 0.5f;

        GameObject redIndicator = GameObject.Instantiate(redIndicatorPrefab, transform.position + redIndicatorPosOffset * 2.0f, Quaternion.identity);
        Destroy(redIndicator, timeToDestroy);
    }

    private void StartJump()
    {
        //Change enum state
        spiderState = SpiderBehaviourStates.Jump;

        originalPosition = transform.position;

        GetComponent<Collider>().enabled = false;

        ResetAllAnimationTriggers();
        animator.SetTrigger("ToJump");
    }

    private void Explode()
    {
        AudioManager.PlaySound("SpiderExplode", 0.4f, Random.Range(0.75f, 1.25f));

        var colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider hit in colliders)
        {
            if(hit.CompareTag("Player"))
            {
                //Player parts are all tagged as "Player", check for PlayerHealth script
                PlayerHealth playerHealthScript = hit.gameObject.GetComponent<PlayerHealth>();

                if (playerHealthScript != null)
                {
                    GetComponent<Damage>().DealDamage(hit);
                    playerHealthScript.StartCoroutine(
                        playerHealthScript.ApplyDamageOverTime(
                            explodeDamagePerTick, explodeDamageNumTicks, explodeDamageTotalDuration, explodeDamageColor));

                    GameObject poisonLingerObject = Instantiate(poisonLingerParticlePrefab, hit.transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)));
                    poisonLingerObject.GetComponent<FollowObject>().SetFollowObject(hit.gameObject);
                    ParticleSystem.MainModule poisonLingerParticleSystem = poisonLingerObject.GetComponent<ParticleSystem>().main;
                    poisonLingerParticleSystem.startLifetime = explodeDamageTotalDuration;
                    poisonLingerParticleSystem.duration = explodeDamageTotalDuration * 1.25f;
                    Destroy(poisonLingerObject, explodeDamageTotalDuration * 2.0f);

                    break; //Player has two colliders, just apply damage once 
                }
            }
        }

        GameObject poisonCloudParticle = GameObject.Instantiate(explodeParticlePrefab, transform.position, Quaternion.identity);
        ParticleSystem poisonCloudParticleSystem = poisonCloudParticle.GetComponent<ParticleSystem>();

        poisonCloudParticleSystem.startSize = explodeRadius + 4;
        poisonCloudParticleSystem.startLifetime = explodeDuration;
        poisonCloudParticle.GetComponent<ParticleSystem>().Play();
        Destroy(poisonCloudParticle, 1.0f);
        StartCoroutine(StopParticle(poisonCloudParticleSystem, explodeDuration));
        ScreenShakeManager.Instance.ScreenShake(0.25f, 0.4f);
        Destroy(gameObject);
    }

    IEnumerator StopParticle(ParticleSystem system, float time)
    {
        yield return new WaitForSeconds(time);
        system.Stop();
    }

    private void ResetAllAnimationTriggers()
    {
        animator.ResetTrigger("ToWindup");
        animator.ResetTrigger("ToJump");
    }

    private void ResetWaitTicks()
    {
        wait = waitTicks;
    }
}
