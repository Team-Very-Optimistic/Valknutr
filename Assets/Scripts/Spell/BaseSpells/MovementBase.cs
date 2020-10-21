﻿using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;

class MovementBase : SpellBase
{
    private ThirdPersonCharacter _controller;
    public float _moveTime = 0.3f;
    
    // protected override void SetValues()
    // {
    //     _moveTime = 0.3f;
    //     objectForSpell = _player.gameObject;
    //     _controller = objectForSpell.GetComponent<ThirdPersonCharacter>();
    //     offset = 2 * (Random.value < 0.5f ? Vector3.left : Vector3.right);
    // }

    public override SpellContext GetContext()
    {
        var ctx = base.GetContext();
        ctx.objectForSpell = _player.gameObject;
        return ctx;
    }

    /// <summary>
    /// todo: use the following properties:
    /// _direction: yes
    /// _objectForSpell: yes kinda
    /// _speed: yes
    /// _damage: Nope
    /// _offset: Nope
    /// _objectsCollided: yes 
    /// _trigger: yes
    /// </summary>
    public override void SpellBehaviour(SpellContext ctx)
    {
        var _controller = ctx.objectForSpell.GetComponent<ThirdPersonCharacter>();
        ctx.direction.y = 0;
        if(!_controller.m_Dashing)
            _controller.Dash(_moveTime * 30f / ctx.speed, ctx.speed, ctx.direction);
        else
        {
            var illu = Instantiate(ctx.objectForSpell, ctx.objectForSpell.transform.position + ctx.offset,
                Quaternion.Euler(new Vector3()));
            
            var thirdPersonCharacter = illu.GetComponent<ThirdPersonCharacter>();
            illu.GetComponent<SpellCaster>().enabled = false;

            thirdPersonCharacter.transformChild.eulerAngles = new Vector3();
            thirdPersonCharacter.m_Dashing = false;
            //thirdPersonCharacter.Dash(_moveTime * 30f / _speed, _speed, _direction);
            illu.AddComponent<Illusion>();
            ctx.objectForSpell = illu;
        }
    }
    
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip($"Dash {DefaultBaseTitle(ctx)}", $"Dash quickly in any direction in {_moveTime * 30f / ctx.speed:F}s. {DefaultBaseBody(ctx)}");
    }
}

public class Illusion : MonoBehaviour
{
    [HideInInspector]
    private float timeAlive = 6f;
    
    public void Start()
    {
        Destroy(gameObject, timeAlive);
    }
}