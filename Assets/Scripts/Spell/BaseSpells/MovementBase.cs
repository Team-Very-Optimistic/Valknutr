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
        _objectForSpell = _player.gameObject;
        _controller = _objectForSpell.GetComponent<ThirdPersonCharacter>();
        animationType = CastAnimation.Movement;
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
        // _objectForSpell.GetComponent<ThirdPersonCharacter>().Move(_posDiff * _speed, false, false);
        _controller.Dash(_moveTime * 30f / _speed, _speed, _direction);
    }
}