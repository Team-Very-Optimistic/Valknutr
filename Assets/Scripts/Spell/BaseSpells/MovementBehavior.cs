using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

class MovementBehavior : SpellBehavior
{
    private ThirdPersonCharacter _controller;
    private float _moveTime = 0.395f;
    
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
        AudioManager.PlaySoundAtPosition("dash", _controller.transform.position);
        _posDiff.y = 0;
        //Todo: make dash better
        // _objectForSpell.GetComponent<ThirdPersonCharacter>().Move(_posDiff * _speed, false, false);
        _controller.Dash(_moveTime, _speed, _posDiff);
    }
}