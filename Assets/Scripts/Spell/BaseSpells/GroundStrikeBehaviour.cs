
using UnityEngine;

public class GroundStrikeBehaviour : SpellBehavior
{
    public float radius = 2F;
    public float power = 50.0F;
    private Damage damageScript;
    public Vector3 offset;
    public float knockbackForce = 1500.0f;

    public override void Init()
    {
        _damage = 4f;
        _speed = 2f;
        _cooldown = .2f;
        
        _objectForSpell = GameManager.Instance._weapon;
        radius = _objectForSpell.transform.localScale.x * 1.5f;
        animationType = CastAnimation.Projectile;
        damageScript = _objectForSpell.GetComponent<Damage>();
        offset = Vector3.down;
    }

    public override void SpellBehaviour(Spell spell)
    {
        var position = _objectForSpell.transform.position + _posDiff;
        
        ScreenShakeManager.Instance.ScreenShake(0.1f, 0.1f);
        AudioManager.PlaySoundAtPosition("groundStrike", position);
        EffectManager.PlayEffectAtPosition("groundStrike", position + offset);

        var cols = Physics.OverlapSphere(position, radius);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        foreach (var col in cols)
        {
            if (!col.CompareTag("Player") && col.attachedRigidbody != null)
            {
                if(col.gameObject.GetComponent<EnemyBehaviourBase>() != null)
                {
                    //Enable knockback on enemies
                    col.gameObject.GetComponent<EnemyBehaviourBase>().EnableKnockback(true);
                }

                //Add knockback direction based on player position
                Vector3 knockbackDirection = (col.transform.position - player.transform.position).normalized;
                col.attachedRigidbody.AddForce(knockbackDirection * knockbackForce); 
            }

            if (!col.CompareTag("Player") && !col.CompareTag("Projectile"))
            {
                damageScript.SetDamage(_damage);
                damageScript.DealDamage(col);
            }
        }
        
    }
}
