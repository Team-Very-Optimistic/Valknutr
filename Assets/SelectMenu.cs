using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenu : MonoBehaviour
{
    public RectTransform pointerPivot;

    public UISlot[] UISlots;
    private UISlot currentlySelectedSlot;
    private int count;
    public GameObject prefab;
    private SpellCaster m_spellCaster;
    private HashSet<Spell> _addedSpells;
    
    // Start is called before the first frame update
    void Start()
    {
        _addedSpells = new HashSet<Spell>();
        count = 0;
        foreach (var uiSlots in UISlots)
        {
        }
        InvokeRepeating( nameof(IntervalUpdate), 0f, 0.1f);

        m_spellCaster = GameManager.Instance._player.GetComponent<SpellCaster>();
    }

    public void UpdateMenu()
    {
        if (count > 9)
        {
            return;
        }
        foreach (var spell in Inventory.Instance._spells)
        {
            if (_addedSpells.Contains(spell))
            {
                return;
            }
            var newObj = Instantiate(prefab, transform);
            var uiItem = newObj.GetComponent<UIItem>();
            uiItem._spellItem = spell;
            uiItem.SetImage();
            UISlots[count++].Slot(uiItem);
            _addedSpells.Add(spell);
        }
    }

    //todo: keybindings
    public void Update()
    {
        var index = -1;
        if (Input.GetKey(KeyCode.Q))
        {
            index = 2;
        }
        if (Input.GetKey(KeyCode.E))
        {
            index = 3;

        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            index = 0;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            index = 1;
        }

        if (index <= 0) return;
        m_spellCaster.SetSpell(index, (Spell) currentlySelectedSlot.GetSlottedItem());
    }

    // Update is called once per 0.1s
    void IntervalUpdate()
    {
        currentlySelectedSlot = GetClosest(UISlots, Input.mousePosition);
        var pointerDirection = currentlySelectedSlot.transform.position - pointerPivot.position;
        float targetRotation = 180 / Mathf.PI * Mathf.Atan2(pointerDirection.y,pointerDirection.x) + 90f;
        
        pointerPivot.eulerAngles = new Vector3(0,0, targetRotation);
        
    }

    private UISlot GetClosest(UISlot[] allSlots, Vector3 pos)
    {
        UISlot closest = null;
        float closeVal = Single.MaxValue;
        
        foreach (var slot in allSlots)
        {
            var position = slot.transform.position;
            var sqrMagnitude = (pos - position).sqrMagnitude;
            if (sqrMagnitude < closeVal)
            {
                closeVal = sqrMagnitude;
                closest = slot;
            }
        }

        return closest;
    }
}
    