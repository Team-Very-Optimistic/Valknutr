using UnityEngine;

public class EnemyBehaviour_Wolf : EnemyBehaviourBase
{
    enum WolfBehaviourStates
    {
        Running,
        Windup,
        Charging,
        Rest
    }

    private WolfBehaviourStates wolfState;

    //Dashing Parameters
    public float dashWindupTime;
    private Vector3 dashDirection;
    [SerializeField] private AnimationCurve dashCurve;

    //Resting Parameter
    public float restTime;

    //Resetting Parameters
    private float distanceToDashLocation;
    private float originalStoppingDistance;
    private float originalSpeed;

    //Collider for dash
    private Collider wolfCollider;
    private Rigidbody wolfRigidbody;
    private float chargeDuration = 1.0f;
    private float chargeTimeElapsed = 0.0f;

    public GameObject redIndicatorPrefab;
    private Vector3 redIndicatorPosOffset;

    //Wait frames between states - NavMeshAgent SetDestination bug: remaining distance always starts off at zero
    int wait = 0, waitTicks = 1;

    public override void Start()
    {
        base.Start();

        //Set running state
        wolfState = WolfBehaviourStates.Running;

        //Get original stopping distance and speed
        originalStoppingDistance = navMeshAgent.stoppingDistance;
        originalSpeed = navMeshAgent.speed;

        //Set collider
        wolfCollider = GetComponent<Collider>();
        wolfRigidbody = GetComponent<Rigidbody>();

        redIndicatorPosOffset = new Vector3(0.0f, wolfCollider.bounds.size.y * 2.0f, 0.0f);

        //Disable knockback (buggy with navmesh)
        canKnockback = false;
    }

    public override void Update()
    {
        switch (wolfState)
        {
            case WolfBehaviourStates.Running:
                {
                    if (!isInKnockback)
                    {
                        //Navigation
                        navMeshAgent.SetDestination(player.transform.position);
                    }

                    //If close enough to player, switch to wind up
                    if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                    {
                        SetupDash();
                    }

                    break;
                }

            case WolfBehaviourStates.Windup:
                {
                    //Transition to Charging by invoking StartDash in SetupDash
                    break;
                }

            case WolfBehaviourStates.Charging:
                {
                    //Change rigidbody velocity
                    chargeTimeElapsed += Time.deltaTime;
                    wolfRigidbody.velocity = dashDirection * dashCurve.Evaluate(chargeTimeElapsed) * 7.0f;

                    //If close enough to dash location, start resting
                    if (chargeTimeElapsed >= chargeDuration)
                    {
                        StartRest();
                    }

                    break;
                }

            case WolfBehaviourStates.Rest:
                {
                    //Transition to running state by invoking StartRunning by StartRest
                    transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y,
                        player.transform.position.z));
                    break;
                }
        }
    }

    private void SetupDash()
    {
        //Set direction to dash towards
        dashDirection = (player.transform.position - transform.position).normalized;
        dashDirection = new Vector3(dashDirection.x, 0.0f, dashDirection.z);

        //Set rotation to player when engaging (use enemy y to prevent vertical rotation)
        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

        //Temporarily disable navMeshAgent
        navMeshAgent.enabled = false;

        //Invoke show red indicator function + start charge function
        Invoke(nameof(ShowRedIndicator), dashWindupTime * 0.5f);
        Invoke(nameof(StartDash), dashWindupTime);

        //Change enum state
        wolfState = WolfBehaviourStates.Windup;

        //Trigger anim state
        ResetWaitTicks();
        ResetAllAnimationTriggers();
        animator.SetTrigger("ToWindup");
    }

    private void ShowRedIndicator()
    {
        float timeToDestroy = dashWindupTime * 0.5f;

        GameObject redIndicator = GameObject.Instantiate(redIndicatorPrefab, transform.position + redIndicatorPosOffset,
            Quaternion.identity);
        Destroy(redIndicator, timeToDestroy);
    }

    private void StartDash()
    {
        wolfRigidbody.isKinematic = false;
        wolfRigidbody.useGravity = false;

        //Change enum state
        wolfState = WolfBehaviourStates.Charging;

        //Disable knockback
        //canKnockback = false;

        //Trigger anim state
        ResetAllAnimationTriggers();
        animator.SetTrigger("ToCharge");
    }

    private void StartRest()
    {
        //Change enum state
        wolfState = WolfBehaviourStates.Rest;

        //Enable knockback
        //canKnockback = true;
        ResetRigidBody();
        navMeshAgent.enabled = true;
        chargeTimeElapsed = 0.0f;

        Invoke(nameof(StartRunning), restTime);

        ResetAllAnimationTriggers();
        animator.SetTrigger("ToRest");
    }

    private void StartRunning()
    {
        //Change enum state
        wolfState = WolfBehaviourStates.Running;

        navMeshAgent.stoppingDistance = originalStoppingDistance;
        navMeshAgent.speed = originalSpeed;

        ResetAllAnimationTriggers();
        animator.SetTrigger("ToRun");
    }

    public void OnTriggerEnter(Collider other)
    {
        //Only trigger when charging - Cannot toggle boxcollider + isTrigger
        if (wolfState == WolfBehaviourStates.Charging)
        {
            //Check only player's capsule collider
            if (other.gameObject.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider) ||
               (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Player")))
            {
                gameObject.GetComponentInParent<Damage>().DealDamage(other);
            }
        }
    }

    private void ResetAllAnimationTriggers()
    {
        animator.ResetTrigger("ToRun");
        animator.ResetTrigger("ToWindup");
        animator.ResetTrigger("ToCharge");
        animator.ResetTrigger("ToRest");
    }

    private void ResetWaitTicks()
    {
        wait = waitTicks;
    }

    private void ResetRigidBody()
    {
        wolfRigidbody.isKinematic = true;
        wolfRigidbody.useGravity = true;
    }
}