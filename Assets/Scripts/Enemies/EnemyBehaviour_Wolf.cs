using System.Collections;
using System.Collections.Generic;
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
    private Vector3 dashLocation;
    [SerializeField] private AnimationCurve dashCurve;

    //Resting Parameter
    public float restTime;

    //Resetting Parameters
    private float distanceToDashLocation;
    private float originalStoppingDistance;
    private float originalSpeed;

    //Collider for dash
    private Collider wolfCollider;

    public GameObject redIndicatorPrefab;
    private Vector3 redIndicatorPosOffset;
    
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

        redIndicatorPosOffset = new Vector3(0.0f, wolfCollider.bounds.size.y, 0.0f);
    }

    public override void Update()
    {
        if (navMeshAgent.enabled) // Not in knockback
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
                        //Change navMeshAgent speed according to curve multiplier
                        float distanceLeftToDashLocation = (transform.position - dashLocation).magnitude;
                        navMeshAgent.speed = originalSpeed * dashCurve.Evaluate(1.0f - (distanceLeftToDashLocation / distanceToDashLocation));

                        //If close enough to dash location, start resting
                        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                        {
                            StartRest();
                        }
                        break;
                    }
                 
                case WolfBehaviourStates.Rest:
                    {
                        //Transition to running state by invoking StartRunning by StartRest
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
                        break;
                    }
            }
        }
        else
        {
            //Check in knockback state before stopping knockback state - Velocity update not neccesarily within same frame of enableknockback
            if (!isInKnockback)
            {
                if (GetComponent<Rigidbody>().velocity.magnitude > 0.0f)
                {
                    isInKnockback = true;
                }
            }
            else
            {
                if (GetComponent<Rigidbody>().velocity.magnitude <= knockbackVelStoppingThreshold)
                {
                    EnableKnockback(false);
                    isInKnockback = false;
                }
            }
        }
    }

    private void SetupDash()
    {
        //Set location to dash to
        Vector3 direction = (player.transform.position - transform.position).normalized;
        dashLocation = transform.position + direction * 2.0f * originalStoppingDistance;

        //Set rotation to player when engaging (use enemy y to prevent vertical rotation)
        transform.LookAt(new Vector3(dashLocation.x , this.transform.position.y, dashLocation.z));

        //Temporarily disable navMeshAgent
        navMeshAgent.enabled = false;

        //Invoke show red indicator function + start charge function
        Invoke(nameof(ShowRedIndicator), dashWindupTime * 0.5f);
        Invoke(nameof(StartDash), dashWindupTime);

        //Change enum state
        wolfState = WolfBehaviourStates.Windup;
    }

    private void ShowRedIndicator()
    {
        float timeToDestroy = dashWindupTime * 0.5f;

        GameObject redIndicator = GameObject.Instantiate(redIndicatorPrefab, transform.position + redIndicatorPosOffset, Quaternion.identity);
        Destroy(redIndicator, timeToDestroy);
    }

    private void StartDash()
    {
        //Enable navMeshAgent
        navMeshAgent.enabled = true;

        //Change enum state
        wolfState = WolfBehaviourStates.Charging;

        //Change stopping distance to smaller range
        navMeshAgent.stoppingDistance = 0.05f;

        //Navigation
        navMeshAgent.SetDestination(dashLocation);

        //Set overall distance
        distanceToDashLocation = (transform.position - dashLocation).magnitude;
    }

    private void StartRest()
    {
        //Change enum state
        wolfState = WolfBehaviourStates.Rest;

        Invoke(nameof(StartRunning), restTime);
    }

    private void StartRunning()
    {
        //Change enum state
        wolfState = WolfBehaviourStates.Running;

        navMeshAgent.stoppingDistance = originalStoppingDistance;
        navMeshAgent.speed = originalSpeed;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Only trigger when charging - Cannot toggle boxcollider + isTrigger
        if(wolfState == WolfBehaviourStates.Charging)
        {
            if (!other.gameObject.CompareTag("Enemy"))
            {
                this.gameObject.GetComponentInParent<Damage>().DealDamage(other);
            }
        }
    }
}
