using UnityStandardAssets.Characters.ThirdPerson;

class MovementSpell : SpellBaseType
{
    private ThirdPersonCharacter _controller;
    private float _moveTime = 0.3f;
    public override void Init()
    {
        _speed = 25f;
        _cooldown = 0.8f;

        _objectForSpell = GameManager.Instance._player;
        _controller = _objectForSpell.GetComponent<ThirdPersonCharacter>();
    }
    public override void SpellBehaviour(Spell spell)
    {
        _posDiff.y = 0;
        //Todo: make dash better
        // _objectForSpell.GetComponent<ThirdPersonCharacter>().Move(_posDiff * _speed, false, false);
        _controller.Dash(_moveTime, _speed, _posDiff);
    }
}