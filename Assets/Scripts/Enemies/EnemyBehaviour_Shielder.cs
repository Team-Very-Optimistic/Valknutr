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

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Invoke(nameof(FindAllEnemies), 0.1f);
    }

    // Update is called once per frame
    public override void Update()
    {
        if(enemiesFound)
        {
            //Update list
            enemies = enemies.Where(enemy => enemy != null).ToList();

            navMeshAgent.SetDestination(FindOptimalPosition());

            foreach(GameObject enemy in enemies)
            {
                if((transform.position - enemy.transform.position).magnitude <= shieldRadius)
                {
                    if(enemy.GetComponent<EnemyShielder_Link>() == null)
                    {
                        EnemyShielder_Link linkScript = enemy.AddComponent<EnemyShielder_Link>();
                        linkScript.InitShielderLink(gameObject, shieldRadius, linkMat, linkDamageMultiplier);
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

        Vector3 directionalPosition = new Vector3(0.0f, 0.0f, 0.0f);

        //Get position in direction of shielder
        foreach (GameObject enemy in enemies)
        {
            directionalPosition += (optimalPosition - enemy.transform.position).normalized * (shieldRadius * 0.5f);
        }

        return optimalPosition + directionalPosition;
    }
}
