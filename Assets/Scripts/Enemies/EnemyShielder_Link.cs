using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShielder_Link : MonoBehaviour
{
    private GameObject enemyShielder;
    private GameObject enemyShielderLink;
    private float enemyShielderRadius;

    void Update()
    {
        if(enemyShielder != null)
        {
            enemyShielderLink.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            enemyShielderLink.GetComponent<LineRenderer>().SetPosition(1, enemyShielder.transform.position);

            //Consider doing raycast (ignore enemy/player colliders)
            if((enemyShielder.transform.position - transform.position).magnitude > enemyShielderRadius)
            {
                Destroy(this);
            }

            //If enemy health = 0, destroy this as well
            if(this.GetComponent<HealthScript>().GetHealth() <= 0.0f)
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
        LineRenderer lRend = enemyShielderLink.AddComponent<LineRenderer>();
        lRend.material = linkMat;
        lRend.startWidth = 0.02f;
        lRend.endWidth = 0.02f;

        GetComponent<HealthScript>().SetDamageMultiplier(damageMultiplier);
    }

    void OnDestroy()
    {
        GetComponent<HealthScript>().ResetDamageMultiplier();
        Destroy(enemyShielderLink);
    }
}
