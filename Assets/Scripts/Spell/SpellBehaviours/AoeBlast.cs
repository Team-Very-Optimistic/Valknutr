using System.Collections;
using UnityEngine;
 
///summary
///summary
public class AoeBlast : SpellBehaviour
{
	#region Public Fields
	[HideInInspector]
    public GameObject _aoeEffect;

    private Collider _collider;
    private Damage _damage;
    [SerializeField] private float time;
    [SerializeField] private float duration;

    #endregion
 


    public override void TriggerEvent(Collider other)
    {
	    if (other.gameObject != GameManager.Instance._player)
	    {
		    _damage.DealDamage(other);
		    _collider.enabled = false;
		    StartCoroutine(EnableCollider(time));
	    }
    }

    private IEnumerator EnableCollider(float time)
    {
	    yield return new WaitForSeconds(time);
	    _collider.enabled = true;
    }

    public override void SetProperties(float damage,  float scale, float speed, float cooldown, params float[] add)
    {
	    _aoeEffect = SpellManager.Instance.aoeEffect;
	    _aoeEffect = Instantiate(_aoeEffect, gameObject.transform);
	    _collider = _aoeEffect.GetComponentElseAddIt<SphereCollider>();
	    _damage = _aoeEffect.GetComponentElseAddIt<Damage>();
	    _damage.SetDamage(damage);
	    time = add[0];
	    duration = add[1];
	    Destroy(this, duration);
	    Destroy(_aoeEffect, duration);
    }
}