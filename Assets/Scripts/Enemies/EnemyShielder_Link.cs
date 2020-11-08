using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShielder_Link : MonoBehaviour
{
    private GameObject enemyShielder;
    private GameObject enemyShielderLink;
    private GameObject shielderShield;
    private float enemyShielderRadius;
    private HealthScript health;
    private LineRenderer line;
    private Color shieldDamageColor = new Color(0.0f, 0.98f, 1.0f, 1.0f);

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
            if(health && health.GetHealth() <= 0.0f)
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

        shielderShield = GameObject.Instantiate(GameManager.Instance.shielderShieldPrefab, gameObject.transform);
        float scaleMultiplier = Mathf.Floor(GetComponentInChildren<SkinnedMeshRenderer>().bounds.extents.x);
        if (scaleMultiplier < 1) scaleMultiplier = 1.0f;
        shielderShield.transform.localScale *= scaleMultiplier;

        health = GetComponent<HealthScript>();
        health.AdditivelyAddDmgMultiplier(damageMultiplier);
        health.SetHasShield(true);
        health.SetShieldDamageColor(shieldDamageColor);
    }

    void OnDestroy()
    {
        health.ResetDamageMultiplier();
        health.SetHasShield(false);
        Destroy(enemyShielderLink);
        Destroy(shielderShield);
    }

    public void RemoveSelfFromShielderList()
    {
        enemyShielder.GetComponent<EnemyBehaviour_Shielder>().RemoveEnemyFromList(gameObject);
    }
}
