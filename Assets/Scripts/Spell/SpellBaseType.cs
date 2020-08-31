using UnityEngine;

[System.Serializable]
public abstract class SpellBaseType
{
    [SerializeField]
    public GameObject _objectForSpell; //can be anything
    [SerializeField]
    public Vector3 _posDiff;
    
    
    public virtual void Init(){}

    public abstract void SpellBehaviour(Spell spell);

}