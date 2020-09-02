using System;
using System.Collections;
using UnityEngine;
using Object = System.Object;
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
        public bool isInitializer;
        
        private void Start()
        {
            if (isInitializer)
            {
                fire = SpellManager.Instance.fireObject;
                fire = Instantiate(fire, gameObject.transform.position, gameObject.transform.rotation);
                fire.AddComponent<Fire>();
                fire.transform.SetParent(gameObject.transform);
                Destroy(this);
            }
            else{
                StartCoroutine(WaitCooldown(0.01f));
                // Destroy(fire, 10f);
                // Destroy(this, 1.5f);
                Destroy(gameObject, 5f);
            }
        }

        /*
         * This doesn't work for now because the collider only chekcs the current game object, need to create new gameobject for this.
         */
        public void OnTriggerEnter(Collider other)
        {
            if (!canSpread) return;
            Debug.Log(other.gameObject.tag);

            if (!gameObject.CompareTag(other.gameObject.tag))
            {
                Debug.Log(other.gameObject.tag);
                other.gameObject.AddComponent<Fire>().isInitializer = true;
            }
            StartCoroutine(WaitCooldown(0.1f));
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
        spell._objectForSpell.AddComponent<Fire>().isInitializer = true;
    }
    // public override SpellBaseType ModifyBehaviour(SpellBaseType action)
    // {
    //     //important to make sure it doesnt cast a recursive method
    //     Action oldBehavior = action.behaviour;
    //     Action spell = () =>
    //     {
    //         var fire = action._objectForSpell.AddComponent<Fire>();
    //         oldBehavior.Invoke();
    //         GameManager.Destroy(fire);
    //
    //     };
    //     action.behaviour = spell;
    //     return action;
    // }
    
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