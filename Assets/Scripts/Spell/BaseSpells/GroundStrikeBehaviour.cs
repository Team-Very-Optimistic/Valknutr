
using UnityEngine;

public class GroundStrikeBehaviour : SpellBehavior
{
    public float radius = 3.0F;
    public float power = 50.0F;
    private Damage damageScript;

    public override void Init()
    {
        _damage = 1f;
        _speed = 2f;
        _cooldown = .2f;
        _objectForSpell = GameManager.Instance._weapon;
        animationType = CastAnimation.Projectile;
        damageScript = _objectForSpell.GetComponent<Damage>();
    }

    public override void SpellBehaviour(Spell spell)
    {
        var position = _objectForSpell.transform.position;


        AudioManager.PlaySoundAtPosition("projectileHit", position);
        EffectManager.PlayEffectAtPosition("groundStrike", position);

        var cols = Physics.OverlapSphere(position, radius);
        
        foreach (var col in cols)
        {
            if (!col.CompareTag("Player") &&  !col.CompareTag("Projectile") && col.attachedRigidbody != null)
            {
                col.attachedRigidbody.AddExplosionForce(power * _damage, position, radius, 0, ForceMode.Impulse);
            }
             damageScript.SetDamage(_damage);
             damageScript.DealDamage(col);
        }
        
    }
}
