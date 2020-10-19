using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : Selectable, IPointerClickHandler
{
    protected CanvasGroup _canvasGroup;
    protected Vector3 _oriPos;
    protected Transform _oriParent;
    public bool isSlotted;
    protected int _siblingIndex;
    public SpellItem _spellItem;
    protected bool selected;
    protected KeyCodeUI _keyCodeUi;


    private void Start()
    {
        _oriParent = transform.parent;
        _oriPos = transform.position;
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_spellItem != null)
        {
            transform.GetChild(1).GetComponent<Image>().sprite = _spellItem._UIsprite;
        }

        if (_spellItem.isBaseSpell)
        {
            GetComponent<Mask>().enabled = false;
            //transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

    //Detect if the Cursor starts to pass over the GameObject
    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_spellItem != null)
            UiManager.ShowTooltip(((ITooltip) _spellItem).GetTooltip());
    }

    //Detect when Cursor leaves the GameObject
    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        UiManager.HideTooltip();
    }

    public void Init()
    {
        if (_spellItem != null)
        {
            transform.GetChild(1).GetComponent<Image>().sprite = _spellItem._UIsprite;
        }
    }
    
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        selected = true;
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) {
            Debug.Log ("double click");
        }
    }
    
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        selected = false;
    }

    public void SetLoose()
    {
        _canvasGroup.blocksRaycasts = true;
        if (!isSlotted)
        {
            transform.SetParent(_oriParent);
            transform.SetSiblingIndex(_siblingIndex);
        }
    }

    public void SetKeyCode(KeyCode key)
    {
        if (_keyCodeUi == null)
        {
            _keyCodeUi = GetComponentInChildren<KeyCodeUI>();
        }
        _keyCodeUi.SetKeyCode(key);
    }
}