using System;
using UnityEngine;
using Random = UnityEngine.Random;

class ShieldBase : SpellBase
{
    private float offsetIncrement;

    // protected override void SetValues()
    // {
    //     offsetIncrement = 45f;
    // }

    public override void SpellBehaviour(SpellContext ctx)
    {
        if (SpellManager.Instance.ShieldFull())
        {
            return;
        }

        var p = Instantiate(objectForSpell, _player.position +
                                                ctx.offset + _player.forward * ctx.speed / 50f * Random.Range(0.6f, 1.4f) * ctx.scale,
            _player.localRotation);
        // float rotateBy = (float) Math.Ceiling(i / 2.0) * (i % 2 == 0 ? -1 : 1) * offsetIncrement;
        var rotateBy = 0f;
        rotateBy += direction.x * 90 + direction.z * 90;
        p.transform.RotateAround(_player.position, Vector3.up, rotateBy);
        p.transform.SetParent(_player);
        p.AddComponent<Shield>().SetSpeed(speed);
        ctx.objectForSpell = p;
    }

    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip($"Shield {DefaultBaseTitle(ctx)}",
            $"Spawns a shield that absorbs {_player.GetComponent<PlayerHealth>().maxHealth / 10 + 10 * ctx.scale:F1} damage for the player. \n{DefaultBaseBody(ctx)}");
    }
}