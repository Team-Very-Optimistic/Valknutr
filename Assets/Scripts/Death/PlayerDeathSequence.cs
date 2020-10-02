using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class PlayerDeathSequence : BaseDeathSequence
{
    [SerializeField]
    private OrthoSmoothFollow _orthoSmoothFollow;
    public void StartDeathSequence()
    {
        TriggerRagdoll();
        StartGameOverSequence();
        SetCameraFocusRagdoll();
        GetComponent<ThirdPersonUserControl>().enabled = false;
        UIInputController.Instance.enabled = false;
    }

    private void StartGameOverSequence()
    {
        //use this instead
        EndGameManager.Instance.StartGameOverSequence();
    }

    private void SetCameraFocusRagdoll()
    {
       _orthoSmoothFollow.target = ragdollParts[0].transform;
    }
}
