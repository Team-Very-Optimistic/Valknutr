using UnityEngine;

public struct SpellCastData
{
    public GameObject caster;
    public Vector3 castPosition;
    public Vector3 castDirection;
        
    public SpellCastData(GameObject caster, Vector3 castPosition, Vector3 castDirection)
    {
        this.caster = caster;
        this.castPosition = castPosition;
        this.castDirection = castDirection;
    }
}
