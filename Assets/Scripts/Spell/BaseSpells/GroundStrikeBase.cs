﻿
using UnityEngine;

public class GroundStrikeBase : SpellBase
{
    // todo: scale with scale and dmg
    public float radius = 2F;
    public float power = 1000.0F;
    private Damage damageScript;
    
    protected override void SetValues()
    {
        _objectForSpell = GameManager.Instance._weapon;
        if(!damageScript)
            damageScript = _objectForSpell.GetComponent<Damage>();
    }
    
    protected override void SpellEffects(bool screenshake, float duration = 0.1f, float intensity = 0.2f, Vector3 pos = default)
    {
        if (screenshake)
        {
            ScreenShakeManager.Instance.ScreenShake(duration, intensity);
        }
        
        AudioManager.PlaySoundAtPosition("groundStrike", pos, 0, Random.Range(0.8f, 1.3f));
        EffectManager.PlayEffectAtPosition("groundStrike", pos + _offset, 
            new Vector3(_scale,_scale,_scale));
        EffectManager.Instance.UseStaffEffect();
    }
    
    /// <summary>
    /// todo: use the following properties:
    /// _direction: yes
    /// _objectForSpell: yes
    /// _speed: yes
    /// _damage: yes
    /// _offset: yes
    /// _objectsCollided: 
    /// _trigger: yes
    /// </summary>
    public override void SpellBehaviour(Spell spell)
    {
        radius = _scale * 1.5f;
        var position = _objectForSpell.transform.position + _direction * _scale;
        position.y = Mathf.Max(position.y, 1.6f); //will not work with lower terrain
        var intensity = 0.1f * _damage / properties._damage + 0.05f * _scale / properties._scale;
        
        SpellEffects(true, 0.1f, intensity, position);
        
        var cols = Physics.OverlapSphere(position, radius);
        
        foreach (var col in cols)
        {
            if (!col.CompareTag("Player") && !col.CompareTag("Projectile"))
            {
                damageScript.SetDamage(_damage);
                damageScript.DealDamage(col);
            }

            if (!col.CompareTag("Player") && col.attachedRigidbody != null)
            {
                if (col.gameObject.GetComponent<EnemyBehaviourBase>() != null)
                {
                    //Enable knockback on enemies
                    col.gameObject.GetComponent<EnemyBehaviourBase>().EnableKnockback(true);
                }

                //Add knockback direction based on player position
                Vector3 knockbackDirection = (col.transform.position - _player.transform.position).normalized;
                knockbackDirection.y = 0.0f;
                col.attachedRigidbody.AddForce(knockbackDirection * (power * (_damage / properties._damage) * _scale));
            }
        }
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip($"Strike {DefaultBaseTitle()}", $"Strikes the ground with a charged staff, dealing {_damage:F} to entities in a radius of {radius:F}. {DefaultBaseBody()}");
    }
}
