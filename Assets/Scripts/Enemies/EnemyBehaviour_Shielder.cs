using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviour_Shielder : EnemyBehaviourBase
{
    public float shieldRadius;

    public List<GameObject> enemies;
    private bool enemiesFound = false;

    public Material linkMat;

    public float linkDamageMultiplier;

    public override void Start()
    {
        base.Start();

        Invoke(nameof(FindAllEnemies), 0.1f);
    }

    public override void Update()
    {
        if(enemiesFound)
        {
            //Update list
            enemies = enemies.Where(enemy => enemy != null).ToList();

            if (navMeshAgent.enabled)
            {
                navMeshAgent.SetDestination(FindOptimalPosition());
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

            foreach(GameObject enemy in enemies)
            {
                EnemyShielder_Link shielderLinkScript = enemy.GetComponent<EnemyShielder_Link>();

                if ((transform.position - enemy.transform.position).magnitude <= shieldRadius)
                {
                    if (shielderLinkScript == null)
                    {
                        EnemyShielder_Link linkScript = enemy.AddComponent<EnemyShielder_Link>();
                        linkScript.InitShielderLink(gameObject, shieldRadius, linkMat, linkDamageMultiplier);
                    }
                }
                else
                {
                    if (shielderLinkScript != null)
                    {
                        Destroy(shielderLinkScript);
                    }
                }
            }
        }
    }

    private void FindAllEnemies()
    {
        GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemiesArray)
        {
            if (enemy.transform.parent == null && enemy != gameObject)
            {
                enemies.Add(enemy);
            }
        }

        enemiesFound = true;
    }

    private Vector3 FindOptimalPosition()
    {
        Vector3 optimalPosition = new Vector3(0.0f, 0.0f, 0.0f);

        //Mean of location of all enemies
        foreach (GameObject enemy in enemies)
        {
            optimalPosition += enemy.transform.position;
        }

        optimalPosition /= enemies.Count;

        //Get a vector away from the player
        Vector3 directionalPosition = (transform.position - player.transform.position).normalized * (0.25f * shieldRadius);

        return optimalPosition + directionalPosition;
    }

    private void OnDestroy()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyShielder_Link>() != null)
            {
                Destroy(enemy.GetComponent<EnemyShielder_Link>());
            }
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
