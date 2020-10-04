
using UnityEngine;

public class GroundStrikeBase : SpellBase
{
    public float radius = 2F;
    public float power = 1000.0F;
    private Damage damageScript;
    public float scale = 1f;
    public float knockbackForce = 500.0f;
    
    public override void Init()
    {
        _damage = 2f;
        _speed = 2f;
        _cooldown = .2f;
        _objectForSpell = GameManager.Instance._weapon;
        scale = _objectForSpell.transform.localScale.x;
        radius = scale * 1.5f;
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
        var position = _objectForSpell.transform.position + _direction * scale;
        position.y = Mathf.Max(position.y, 1.6f); //will not work with lower terrain
        ScreenShakeManager.Instance.ScreenShake(0.1f, 0.1f);
        AudioManager.PlaySoundAtPosition("groundStrike", position);
        EffectManager.PlayEffectAtPosition("groundStrike", position + _offset, 
            new Vector3(scale,scale,scale));

        
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
                col.attachedRigidbody.AddForce(knockbackDirection * knockbackForce * _damage / 2f * scale);
            }

        }

    }
}
