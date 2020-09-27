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
    
    // Start is called before the first frame update
    void Start()
    {
        
        foreach (var uiSlots in UISlots)
        {
        }
        InvokeRepeating( nameof(IntervalUpdate), 0.1f, 0.1f);
    }

    public void UpdateMenu()
    {
        foreach (var spell in Inventory.Instance._spells)
        {
            var newObj = Instantiate(prefab, transform);
            var uiItem = newObj.GetComponent<UIItem>();
            uiItem._spellItem = spell;
            UISlots[count++].Slot(uiItem);
        }
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
    