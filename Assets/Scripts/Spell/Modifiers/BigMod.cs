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
            float sizeChange = 1.5f;
            oldBehavior.Invoke();
            var transformLocalScale = action._objectForSpell.transform.localScale;
            action._objectForSpell.transform.localScale = transformLocalScale * sizeChange;
            GameManager.Instance.StartCoroutine(MakeSmall(action._objectForSpell, transformLocalScale));
        };
        action.behaviour = spell;
        return action;
    }

    IEnumerator MakeSmall(GameObject obj, Vector3 size)
    {
        if (obj == null)
        {
            
        }
        else if (obj.transform.localScale.magnitude > size.magnitude)
        {
            obj.transform.localScale *= 0.95f;
            yield return new WaitForSeconds(0.15f);
            GameManager.Instance.StartCoroutine(MakeSmall(obj, size));
        }
    } 
    
}