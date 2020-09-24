using System.Collections;
using UnityEngine;

[CreateAssetMenu]
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
        Time.timeScale = _durationScaled;
        TimeSlow(_durationScaled * _duration);
    }
    
    IEnumerator TimeSlow(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
    
}