using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class EnemyBehaviour_Archer : EnemyBehaviourBase
{
    private Vector3 firingPosOffset = new Vector3(0.0f, 90.0f, 0.0f);
    private GameObject bow;

    public GameObject arrowPrefab;

    // Start is called before the first frame update
    public override void Start()
    {
        bow = this.gameObject.transform.Find("Erika_Archer_Meshes").Find("Bow").gameObject;
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            //Offset rotation to properly face player when attacking
            transform.Rotate(firingPosOffset);
        }
    }

    public void FireArrow()
    {
        Vector3 fireDirection = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.gameObject.transform.position;
        Vector3 verticalOffset = new Vector3(0.0f, 1.4f, 0.0f); //bow.transform.position.y returns 0 - Model issue?
        GameObject arrow = GameObject.Instantiate(arrowPrefab, bow.transform.position + verticalOffset, Quaternion.LookRotation(fireDirection));
        arrow.GetComponent<EnemyProjectile>().Launch(fireDirection, 1.75f);
    }
}
