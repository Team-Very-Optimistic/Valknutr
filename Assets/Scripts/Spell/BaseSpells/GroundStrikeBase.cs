
using UnityEngine;

public class GroundStrikeBase : SpellBase
{
    public float radius = 2F;
    public float power = 1000.0F;
    private Damage damageScript;
    private float knockbackScale = 20.0f;
    public float scale = 1f;
    public float damage = 2f;
    
    public override void Init()
    {
        _damage = damage;
        _speed = 2f;
        _cooldown = .2f;
        _objectForSpell = GameManager.Instance._weapon;
        animationType = CastAnimation.Projectile;
        damageScript = _objectForSpell.GetComponent<Damage>();
        _offset = Vector3.down * 1.5f;
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
        ScreenShakeManager.Instance.ScreenShake(0.1f, 0.1f);
        AudioManager.PlaySoundAtPosition("groundStrike", position);
        EffectManager.PlayEffectAtPosition("groundStrike", position + _offset, 
            new Vector3(_scale,_scale,_scale));

        
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
                col.attachedRigidbody.AddForce(knockbackDirection * power * knockbackScale * _damage / 2f * _scale);
            }

        }

    }
}
