using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;

class MovementBase : SpellBase
{
    private ThirdPersonCharacter _controller;
    private float _moveTime = 0.3f;
    
    protected override void SetValues()
    {
        _speed = 30f;
        _cooldown = 1f;
        _moveTime = 0.3f;
        _objectForSpell = _player.gameObject;
        _controller = _objectForSpell.GetComponent<ThirdPersonCharacter>();
        animationType = CastAnimation.Movement;
        _offset = 2 * (Random.value < 0.5f ? Vector3.left : Vector3.right);
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
    public override void SpellBehaviour(Spell spell)
    {
        _direction.y = 0;
        if(!_controller.m_Dashing)
            _controller.Dash(_moveTime * 30f / _speed, _speed, _direction);
        else
        {
            var illu = Instantiate(_objectForSpell, _objectForSpell.transform.position + _offset,
                Quaternion.Euler(new Vector3()));
            
            var thirdPersonCharacter = illu.GetComponent<ThirdPersonCharacter>();
            illu.GetComponent<SpellCaster>().enabled = false;

            thirdPersonCharacter.transformChild.eulerAngles = new Vector3();
            thirdPersonCharacter.m_Dashing = false;
            //thirdPersonCharacter.Dash(_moveTime * 30f / _speed, _speed, _direction);
            illu.AddComponent<Illusion>();
            _objectForSpell = illu;
        }
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