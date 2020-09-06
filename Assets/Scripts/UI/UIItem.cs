using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof (Selectable))]
public class UIItem : Selectable, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private CanvasGroup _canvasGroup;
    private Vector3 _oriPos;
    private Transform _oriParent;
    public bool isSlotted;
    private int _siblingIndex;
    public SpellItem _spellItem;
    
    private void Start()
    {
        _oriParent = transform.parent;
        _oriPos = transform.position;
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_spellItem != null)
        {
            GetComponent<Image>().sprite = _spellItem._UIsprite;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            Debug.Log("Drop item");
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CraftMenuManager.Instance.transform);
        _canvasGroup.blocksRaycasts = false;
        isSlotted = false;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        if (!isSlotted)
        {
            transform.SetParent(_oriParent);
            transform.SetSiblingIndex(_siblingIndex);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        transform.localScale *= 0.8f;   
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("Submit");
            var itemSlots = CraftMenuManager.Instance._itemSlots;

            foreach (var slot in itemSlots)
            {
                if (!slot.IsSlotted())
                {
                    slot.Slot(this);
                }
            }
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        transform.localScale *= 1.25f;
    }
}