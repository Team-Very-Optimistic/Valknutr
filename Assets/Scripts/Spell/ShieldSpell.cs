using System;
using UnityEngine;

class ShieldSpell : SpellBaseType
{
    private Transform player;
    private float offsetIncrement;
    public override void Init()
    {
        offsetIncrement = 45f;
        player = GameManager.Instance._player.transform;
        _objectForSpell = SpellManager.Instance.shieldObject;
    }
    
    public override void SpellBehaviour(Spell spell)
    {
        Debug.Log(spell._spellProperties.iterations);
        for (int i = 0; i < spell._spellProperties.iterations; i++)
        {
            var p = GameObject.Instantiate(_objectForSpell, player.position + Vector3.up + player.forward * 0.7f, player.localRotation);
            float rotateBy = (float) Math.Ceiling(i / 2.0) * (i % 2 == 0 ? -1 : 1) * offsetIncrement;
            p.transform.RotateAround(player.position,Vector3.up, rotateBy);
            p.transform.SetParent(player);
            p.AddComponent<Shield>();
        }
    }
}