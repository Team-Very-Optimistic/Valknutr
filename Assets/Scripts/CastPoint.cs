using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CastPoint : StateMachineBehaviour
{
    private ThirdPersonCharacter _thirdPersonCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_thirdPersonCharacter == null)
        {
            _thirdPersonCharacter = animator.gameObject.GetComponent<ThirdPersonCharacter>();
        }
        // animator.gameObject.SendMessage("CastPoint");
        _thirdPersonCharacter.CastPoint();
    }
}
