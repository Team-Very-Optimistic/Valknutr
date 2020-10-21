using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

class SplitSpellModifier : SpellModifier
{
    private int iterations = 2;
    private float damageReduction = 0.8f;
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
            action._damage *= damageReduction;
            oldBehavior.Invoke();
            action._direction = originalPosDiff; //reset
        };
            
        Action spell = () =>
        {
            for (int i = 0; i < iterations; i++)
            {
                GameManager.Instance.StartCoroutine(DelayInvoke(temp, i / 10f));
            }
        };
        action.behaviour = spell;
        return action;
    }

    public override void UseValue()
    {
        iterations = Mathf.RoundToInt(iterations * value);
        damageReduction = value / (iterations/2);
    }

    IEnumerator DelayInvoke(Action invoke, float delay)
    {
        yield return new WaitForSeconds(delay);
        invoke.Invoke();
    }
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Split" + DefaultModTitle(), 
            $"Repeats the spell effects {iterations} times, but each spell effect is {damageReduction}% weaker. {DefaultModBody()}");
    }
}
