using System;
using System.Collections;
using UnityEngine;

class FireSpellModifier : SpellModifier
{
    class Fire : MonoBehaviour
    {
        private GameObject fire;
        private bool canSpread;
        public bool isInitializer;
        private Vector3 _origPosition;
        
        private void Start()
        {
            if (isInitializer)
            {
                fire = SpellManager.Instance.fireObject;
                if (_origPosition == new Vector3())
                {
                    _origPosition = gameObject.transform.position;
                }
                fire = Instantiate(fire, _origPosition, gameObject.transform.rotation);
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

        public Fire SetInitializer()
        {
            isInitializer = true;
            return this;
        }

        /*
         * This doesn't work for now because the collider only chekcs the current game object, need to create new gameobject for this.
         */
        public void OnTriggerEnter(Collider other)
        {
            if (!canSpread) return;

            if (!gameObject.CompareTag(other.gameObject.tag))
            {
                var closestPointOnBounds = other.ClosestPointOnBounds(transform.position);
                other.gameObject.AddComponent<Fire>().SetInitializer()._origPosition = closestPointOnBounds;
                var damageScript = GetComponent<Damage>();
                damageScript.SetDamage(1);   
                damageScript.DealDamage(other);
            }
            StartCoroutine(WaitCooldown(1.6f));
        }
        
        IEnumerator WaitCooldown(float cooldown)
        {
            canSpread = false;
            yield return new WaitForSeconds(cooldown);
            canSpread = true;
        }

        /*
         * Prevent explosive behaviour
         */
        public void OnDestroy()
        {
            StopAllCoroutines();
        }
    }

    public override void ModifySpell(SpellBehavior spell)
    {
        spell._speed *= 1.2f;
        spell._cooldown *= 1.2f;
    }
    
    public override SpellBehavior ModifyBehaviour(SpellBehavior action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            action._objectForSpell.AddComponent<Fire>().SetInitializer();
        };
        action.behaviour = spell;
        return action;
    }
    
}