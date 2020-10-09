using System;
using System.Collections;
using UnityEngine;

class GrowSpellModifier : SpellModifier
{
    protected float sizeChange = 1.5f;
    protected float cap = 1f;

    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            var transformLocalScale = action._objectForSpell.transform.localScale;
            if (transformLocalScale.x > cap) return;
            action._objectForSpell.transform.localScale = transformLocalScale * sizeChange;
            GameManager.Instance.StartCoroutine(MakeSmall(action._objectForSpell, transformLocalScale));
        };
        action.behaviour = spell;
        return action;
    }

    public override void ModifySpell(SpellBase spell)
    {
        var varSize = Math.Log(spell._cooldown, 1000) + 1.7f;
        sizeChange = (float) Math.Max(1.01f, varSize);;
        base.ModifySpell(spell);
        spell._scale *= sizeChange;
        cap = spell._scale * 2;
    }

    IEnumerator MakeSmall(GameObject obj, Vector3 size)
    {
        if (obj == null)
        {
            
        }
        else if (obj.transform.localScale.magnitude > size.magnitude)
        {
            obj.transform.localScale *= 0.99f;
            yield return new WaitForSeconds(0.15f);
            GameManager.Instance.StartCoroutine(MakeSmall(obj, size));
        }
    } 
    
}