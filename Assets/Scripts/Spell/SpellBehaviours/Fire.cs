using System.Collections;
using UnityEngine;

public class Fire : TriggerEventHandler
{
    private GameObject fire;
    private bool canSpread;
    public bool isInitializer;
    public Vector3 _origPosition;
    private Transform parent;
    private int maxFires = 10;
    public float damage = 1;
    
    public override void TriggerEvent(Collider other)
    {
        if(maxFires < 0) return;
        if (!canSpread) return;
        if (parent.CompareTag(other.tag) || gameObject.CompareTag(other.tag) || other.CompareTag("Fire")) return;
        maxFires--;
        
        var closestPointOnBounds = other.ClosestPointOnBounds(transform.position);
        other.gameObject.AddComponent<Fire>().SetInitializer(maxFires)._origPosition = closestPointOnBounds;
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(damage);   
        damageScript.DealDamage(other);
        
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
            var sound = AudioManager.PlaySoundAtPosition("fire", transform.position);
            sound.tag = "Fire";
            sound.layer = fire.layer;
            sound.transform.SetParent(fire.transform);
            Fire childFire = fire.AddComponent<Fire>();
            childFire.maxFires = maxFires;
            childFire.damage = damage;
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

    public Fire SetInitializer(int maxFires = 10)
    {
        this.maxFires = maxFires;
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