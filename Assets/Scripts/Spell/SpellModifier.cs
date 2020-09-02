using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SpellModifier
{
    public float _cooldownMultiplier = 1;

    public virtual void ModifySpell(SpellBaseType spell)
    {
        
    }

    public virtual SpellBaseType ModifyBehaviour(SpellBaseType action)
    {
        return action; // No change
    }
}

class FireMod : SpellModifier
{
    class Fire : MonoBehaviour
    {
        private GameObject fire;
        private bool canSpread;
        
        private void Start()
        {
            Debug.Log(gameObject.tag);
            fire = SpellManager.Instance.fireObject;
            fire = Instantiate(fire, gameObject.transform.position, gameObject.transform.rotation);
            fire.transform.parent = gameObject.transform;
            WaitCooldown(0.01f);
            Destroy(fire, 2f);
            Destroy(this, 2f);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!canSpread) return;
            if (!gameObject.CompareTag(other.gameObject.tag))
            {
                other.gameObject.AddComponent<Fire>();
            }
            WaitCooldown(0.1f);
        }

        IEnumerator WaitCooldown(float cooldown)
        {
            canSpread = false;
            yield return new WaitForSeconds(cooldown);
            canSpread = true;
        }
        public void Update()
        {
            
        }
    }
    public override void ModifySpell(SpellBaseType spell)
    {
        spell._objectForSpell.AddComponent<Fire>();
    }
    
}
class SplitShotMod : SpellModifier
{
    
    public override SpellBaseType ModifyBehaviour(SpellBaseType action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 originalPosDiff = action._posDiff;
                action._posDiff += new Vector3(Random.Range(-0.1f,0.1f), 0, Random.Range(-0.1f, 0.1f));
                action._posDiff.Normalize();
                oldBehavior.Invoke();
                action._posDiff = originalPosDiff; //reset
            }
        };
        action.behaviour = spell;
        return action;
    }
    
}     