using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSequence : BaseDeathSequence
{
    public void StartDeathSequence()
    {
        base.TriggerRagdoll();
        StartGameOverSequence();
        SetCameraFocusRagdoll();
        Destroy(GetComponent("ThirdPersonUserControl"));
    }

    private void StartGameOverSequence()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().StartGameOverSequence();
    }

    private void SetCameraFocusRagdoll()
    {
       GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OrthoSmoothFollow>().target = ragdollParts[0].transform;
    }
}
