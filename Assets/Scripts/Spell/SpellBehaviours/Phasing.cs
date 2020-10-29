using System.Collections.Generic;
using UnityEngine;

public class Phasing : SpellBehaviour
{
    public float _damage;
    public int _phaseNum = 2;
    public List<TriggerEventHandler> triggers;
    public Damage damageScript;
    public override void TriggerEvent(Collider other)
    {
    }

    public void Start()
    {
        damageScript = gameObject.GetComponentElseAddIt<Damage>();

        var trig = GetComponents<TriggerEventHandler>();
        triggers = new List<TriggerEventHandler>();
        foreach (var t in trig)
        {
            if (t != this)
            {
                t.OverrideEvent(Trigger);
                triggers.Add(t);
            }
        }
    }

    public void AddPhaseAmount(int phaseAmount)
    {
        if (_phaseNum < 0)
        {
            foreach (var t in triggers)
            {
                t.OverrideEvent(Trigger);
            }
        }
        _phaseNum += phaseAmount;
    }
    
    //public bool CanTrigger { get; set; }
    public void Trigger(Collider other)
    {
        damageScript.SetDamage(_damage);   
        damageScript.DealDamage(other);
        _phaseNum--;
        EffectManager.PlayEffectAtPosition("RainbowEffect", transform.position, transform.lossyScale/2f);
        AudioManager.PlaySoundAtPosition("lightBuff", transform.position);
        if (_phaseNum < 0)
        {
            foreach (var t in triggers)
            {
                t.RemoveOverride(Trigger);
            }
        }
    }

    public override void SetProperties(float damage, float scale, float speed, float cooldown, params float[] additionalProperties)
    {
        this._damage = damage;
    }

}