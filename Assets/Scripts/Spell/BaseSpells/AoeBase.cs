using UnityEngine;

[CreateAssetMenu]
public class AoeBase : SpellBase
{
    public override SpellContext GetContext()
    {
        var ctx = base.GetContext();
        ctx.objectForSpell = _player.gameObject;
        return ctx;
    }

    public override void SpellBehaviour(SpellContext ctx)
    {
        var aoe = ctx.objectForSpell.AddComponent<AoeBlast>();
        aoe.Set(ctx.damage, 0.5f);
        ctx.objectForSpell = aoe._aoeEffect;
    }
    
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip("Aoe <Base>", $"Creates an explosive that detonates after {0.5} seconds, dealing {ctx.damage:F1} to entities in a large radius. Can damage self");
    }
}