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
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Aoe <Base>", $"Creates an explosive that detonates after {0} seconds, dealing {_damage} to entities in a large radius. Can damage self");
    }
}