using System.Collections;
using UnityEngine;

public class Fire : SpellBehaviour
{
    private GameObject fire;
    private bool canSpread;
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
        SpawnFire(other.gameObject, closestPointOnBounds);
        var damageScript = GetComponent<Damage>();
        damageScript.SetDamage(damage);   
        damageScript.DealDamage(other);
        
        StartCoroutine(WaitCooldown(3f));
    }

    protected override void Start()
    {
        base.Start();
        parent = gameObject.transform.parent;
        StartCoroutine(WaitCooldown(0.01f));
        Destroy(gameObject, 5f);
    }

    public static Fire SpawnFire(GameObject parent, Vector3 _origPosition = new Vector3(), int maxFires = 10)
    {
        var fire = SpellManager.Instance.fireObject;
        if(_origPosition == new Vector3())
            _origPosition = parent.transform.position;
        fire = Instantiate(fire, _origPosition, parent.transform.rotation);
        fire.transform.SetParent(parent.transform);
        fire.transform.localScale = parent.transform.lossyScale;
        var sound = AudioManager.PlaySoundAtPosition("fire", _origPosition);
        sound.tag = "Fire";
        sound.layer = fire.layer;
        sound.transform.SetParent(fire.transform);
        Fire childFire = fire.AddComponent<Fire>();
        childFire.maxFires = maxFires;
        fire.transform.SetParent(parent.transform);
        return childFire;
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

    public override void SetProperties(float damage, float scale, float speed, float cooldown, params float[] additionalProperties)
    {
        this.damage = damage/10f;
    }
}