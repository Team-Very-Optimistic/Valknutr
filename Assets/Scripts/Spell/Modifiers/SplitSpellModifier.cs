using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

class SplitSpellModifier : SpellModifier
{
    private int n = 2;
    private float randomMax = 0.2f;
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action temp = () =>
        {
            Vector3 originalPosDiff = action._direction;
            action._direction += new Vector3(Random.Range(-randomMax, randomMax), 0,
                Random.Range(-randomMax, randomMax));
            action._offset += new Vector3(Random.Range(-randomMax, randomMax), 0,
                Random.Range(-randomMax, randomMax));
            action._direction.Normalize();
            oldBehavior.Invoke();
            action._direction = originalPosDiff; //reset
        };
            
        Action spell = () =>
        {
            for (int i = 0; i < n; i++)
            {
                GameManager.Instance.StartCoroutine(DelayInvoke(temp, i / 10f));
            }
        };
        action.behaviour = spell;
        return action;
    }

    IEnumerator DelayInvoke(Action invoke, float delay)
    {
        yield return new WaitForSeconds(delay);
        invoke.Invoke();
    }
}