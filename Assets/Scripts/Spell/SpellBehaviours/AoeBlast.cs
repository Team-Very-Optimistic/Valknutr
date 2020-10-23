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

    public static AoeBlast SpawnBlast(GameObject parent)
    {
	    var instanceAoeEffect = SpellManager.Instance.aoeEffect;
	    instanceAoeEffect = Instantiate(instanceAoeEffect, parent.transform);
	    var blast = instanceAoeEffect.GetComponentElseAddIt<AoeBlast>();
	    blast._aoeEffect = instanceAoeEffect;
	    blast._collider = instanceAoeEffect.GetComponentElseAddIt<SphereCollider>();
	    blast._damage = instanceAoeEffect.GetComponentElseAddIt<Damage>();
	    return blast;
    }
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
	    _damage.SetDamage(damage);
	    time = add[0];
	    duration = add[1];
	    Destroy(this, duration);
	    Destroy(_aoeEffect, duration);
    }
}