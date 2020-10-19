using UnityEngine;

[CreateAssetMenu]
public class SpellItem : ScriptableObject
{
    public Sprite _UIsprite;

    // public GameObject _itemObject;

    public SpellElement _spellElement;

    public bool isBaseSpell;

    public Tooltip GetTooltip()
    {
        if (_spellElement == null)
        {
            Debug.Log("Null spell element");
            return default;
        }
        else
        {
            Debug.Log(_spellElement);
            return _spellElement.GetTooltip();
        }
    }
}