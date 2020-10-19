using UnityEngine;

[CreateAssetMenu]
public class AoeBase : SpellBase
{
    protected override void SetValues()
    {
        _objectForSpell = _player.gameObject;
    }
    public override void SpellBehaviour(Spell spell)
    {
        var aoe = _objectForSpell.AddComponent<AoeBlast>();
        aoe.Set(_damage, 0.5f);
        _objectForSpell = aoe._aoeEffect;
    }
}