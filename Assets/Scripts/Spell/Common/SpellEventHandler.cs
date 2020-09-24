using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SpellEventHandler : MonoBehaviour
{
    public ThirdPersonCharacter character;
    private SpellCaster spellCaster;
    
    public void CastPoint()
    {
        character.CastPoint();
        spellCaster.CastPoint();
    }
}
