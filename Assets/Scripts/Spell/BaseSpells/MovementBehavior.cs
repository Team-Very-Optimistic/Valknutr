using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

class MovementBehavior : SpellBehavior
{
    private ThirdPersonCharacter _controller;
    private float _moveTime = 0.425f;
    
    public override void Init()
    {
        _speed = 30f;
        _cooldown = 1f;

        _objectForSpell = GameManager.Instance._player;
        _controller = _objectForSpell.GetComponent<ThirdPersonCharacter>();
        animationType = CastAnimation.Movement;
    }
    public override void SpellBehaviour(Spell spell)
    {
        _posDiff.y = 0;
        //Todo: make dash better
        // _objectForSpell.GetComponent<ThirdPersonCharacter>().Move(_posDiff * _speed, false, false);
        _controller.Dash(_moveTime, _speed, _posDiff);
    }
}

class TimeSlowBehavior : SpellBehavior
{
    public float _duration;
    public override void Init()
    {
        _speed = 0.2f;
        _cooldown = 8f;

        _duration = 1.5f;

        _objectForSpell = null;
    }
    public override void SpellBehaviour(Spell spell)
    {
        var _durationScaled = _duration * _speed/0.2f;
        Time.timeScale = _speed;
        
    }
    
    IEnumerator TimeSlow(float duaration, float speed)
    {
        yield return null;
    }
    
}