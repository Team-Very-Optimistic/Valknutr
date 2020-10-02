using UnityStandardAssets.Characters.ThirdPerson;

class MovementBase : SpellBase
{
    private ThirdPersonCharacter _controller;
    private float _moveTime = 0.3f;
    
    public override void Init()
    {
        _speed = 30f;
        _cooldown = 1f;
        _moveTime = 0.3f;
        _objectForSpell = GameManager.Instance._player;
        _controller = _objectForSpell.GetComponent<ThirdPersonCharacter>();
        animationType = CastAnimation.Movement;
    }
    public override void SpellBehaviour(Spell spell)
    {
        _direction.y = 0;
        // _objectForSpell.GetComponent<ThirdPersonCharacter>().Move(_posDiff * _speed, false, false);
        _controller.Dash(_moveTime * 30f / _speed, _speed, _direction);
    }
}