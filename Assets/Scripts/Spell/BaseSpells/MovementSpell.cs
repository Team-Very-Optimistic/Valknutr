using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

class MovementSpell : SpellBaseType
{
    private Rigidbody _rb;
    public override void Init()
    {
        _speed = 2500f;
        _cooldown = 0.5f;

        _objectForSpell = GameManager.Instance._player;
        _rb = _objectForSpell.GetComponent<Rigidbody>();
    }
    public override void SpellBehaviour(Spell spell)
    {
        _posDiff.y = 0;
        //Todo: make dash better
        // _objectForSpell.GetComponent<ThirdPersonCharacter>().Move(_posDiff * _speed, false, false);
        _rb.AddForce(_posDiff * _speed);
    }
}