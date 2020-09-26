using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenu : MonoBehaviour
{
    public RectTransform pointerPivot;

    public UISlot[] UISlots;
    private UISlot currentlySelectedSlot;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var uiSlots in UISlots)
        {
            Debug.Log(uiSlots.transform.position);
        }
        InvokeRepeating( nameof(IntervalUpdate), 0.1f, 0.1f);
    }

    // Update is called once per 0.1s
    void IntervalUpdate()
    {
        currentlySelectedSlot = GetClosest(UISlots, Input.mousePosition);
        var pointerDirection = currentlySelectedSlot.transform.position - pointerPivot.position;
        float targetRotation = 180 / Mathf.PI * Mathf.Atan((pointerDirection.y)/(pointerDirection.x));
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
    