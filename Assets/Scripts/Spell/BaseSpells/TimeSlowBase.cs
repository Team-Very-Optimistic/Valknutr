using System.Collections;
using UnityEngine;

[CreateAssetMenu]
class TimeSlowBase : SpellBase
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
        var timeScale = _speed * (_speed/ 0.2f) * (_speed/ 0.2f);
        Time.timeScale = timeScale;
        GameManager.Instance.StartCoroutine(TimeSlow(timeScale * _duration));
    }
    
    IEnumerator TimeSlow(float duration)
    {
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
    
}