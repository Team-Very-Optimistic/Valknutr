using System.Collections;
using UnityEngine;

public class Fire : TriggerEventHandler
{
    private GameObject fire;
    private bool canSpread;
    public bool isInitializer;
    private Vector3 _origPosition;
    private Transform parent;
    public override void TriggerEvent(Collider other)
    {
        if (!canSpread) return;

        if (!parent.CompareTag(other.tag) && !other.CompareTag("Fire"))
        {
            var closestPointOnBounds = other.ClosestPointOnBounds(transform.position);
            other.gameObject.AddComponent<Fire>().SetInitializer()._origPosition = closestPointOnBounds;
            var damageScript = GetComponent<Damage>();
            damageScript.SetDamage(1);   
            damageScript.DealDamage(other);
        }
        StartCoroutine(WaitCooldown(3f));
    }

    protected override void Start()
    {
        base.Start();
        if (isInitializer)
        {
            fire = SpellManager.Instance.fireObject;
            if (_origPosition == new Vector3())
            {
                _origPosition = gameObject.transform.position;
            }
            fire = Instantiate(fire, _origPosition, gameObject.transform.rotation);
            fire.transform.localScale = gameObject.transform.lossyScale;
            AudioManager.PlaySoundAtPosition("fire", transform.position).transform.SetParent(fire.transform);
            fire.AddComponent<Fire>();
            fire.transform.SetParent(gameObject.transform);
            Destroy(this);
        }
        else
        {
            parent = gameObject.transform.parent;
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