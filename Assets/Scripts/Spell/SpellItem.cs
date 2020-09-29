using UnityEngine;

[CreateAssetMenu]
public class SpellItem : ScriptableObject
{
    public Sprite _UIsprite;

    public GameObject _itemObject;

    public SpellElement _spellElement;

    public bool isBaseSpell;

    public string _tooltipMessage;
}