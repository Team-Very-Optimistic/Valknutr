using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SpellEventHandler : MonoBehaviour
{
    public ThirdPersonCharacter character;
    private SpellCastingControl spellCastingControl;
    
    public void CastPoint()
    {
        character.CastPoint();
        spellCastingControl.CastPoint();
    }
}
