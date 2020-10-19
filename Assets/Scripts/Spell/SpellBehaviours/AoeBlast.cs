using System.Collections;
using UnityEngine;
 
///summary
///summary
public class AoeBlast : TriggerEventHandler
{
 
    #region Public Fields
	[HideInInspector]
    public GameObject _aoeEffect;

    private Collider _collider;
    private Damage _damage;
    [SerializeField] private float time;

    #endregion
 
    #region Unity Methods

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

    public void Set(float damage, float time)
    {
	    _aoeEffect = SpellManager.Instance.aoeEffect;
	    _aoeEffect = Instantiate(_aoeEffect, gameObject.transform);
	    _collider = _aoeEffect.GetComponentElseAddIt<Collider>();
	    _damage = _aoeEffect.GetComponentElseAddIt<Damage>();
	    _damage.SetDamage(damage);
	    this.time = time;
    }

    #endregion
 
    #region Private Methods
    #endregion
}