using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class EnemyBehaviour_Archer : EnemyBehaviourBase
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            //Offset rotation to properly face player when attacking
            transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        }
    }
}
