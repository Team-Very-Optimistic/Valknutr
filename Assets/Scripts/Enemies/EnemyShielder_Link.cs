using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShielder_Link : MonoBehaviour
{
    private GameObject enemyShielder;
    private GameObject enemyShielderLink;
    private float enemyShielderRadius;
    private HealthScript health;
    private LineRenderer line;

    void Update()
    {
        if(enemyShielder != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, enemyShielder.transform.position);

            //Consider doing raycast (ignore enemy/player colliders)
            if((enemyShielder.transform.position - transform.position).magnitude > enemyShielderRadius)
            {
                Destroy(this);
            }

            //If enemy health = 0, destroy this as well
            if(health.GetHealth() <= 0.0f)
            {
                Destroy(this);
            }
        }
    }

    public void InitShielderLink(GameObject enemyShielder, float enemyShielderRadius, Material linkMat, float damageMultiplier)
    {
        this.enemyShielder = enemyShielder;
        this.enemyShielderRadius = enemyShielderRadius;

        enemyShielderLink = new GameObject("ShielderLink");
        line = enemyShielderLink.AddComponent<LineRenderer>();
        line.material = linkMat;
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;

        health = GetComponent<HealthScript>();
        health.SetDamageMultiplier(damageMultiplier);
    }

    void OnDestroy()
    {
        health.ResetDamageMultiplier();
        Destroy(enemyShielderLink);
    }
}
