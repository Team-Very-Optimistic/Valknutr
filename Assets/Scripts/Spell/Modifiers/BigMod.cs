using System;
using System.Collections;
using UnityEngine;

class BigMod : SpellModifier
{
    
    public override SpellBaseType ModifyBehaviour(SpellBaseType action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            action._objectForSpell.transform.localScale *= 1.5f;
            oldBehavior.Invoke();
            GameManager.Instance.StartCoroutine(MakeSmall(action._objectForSpell, action._objectForSpell.transform.localScale / 1.5f));

        };
        action.behaviour = spell;
        return action;
    }

    IEnumerator MakeSmall(GameObject obj, Vector3 size)
    {
        if (obj.transform.localScale.magnitude > size.magnitude)
        {
            obj.transform.localScale *= 0.95f;
            yield return new WaitForSeconds(0.15f);
            GameManager.Instance.StartCoroutine(MakeSmall(obj, size));
        }
    } 
    
}