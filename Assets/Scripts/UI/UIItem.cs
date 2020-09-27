using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : Selectable
{
    protected CanvasGroup _canvasGroup;
    protected Vector3 _oriPos;
    protected Transform _oriParent;
    public bool isSlotted;
    protected int _siblingIndex;
    public SpellItem _spellItem;
    protected bool selected;
    private KeyCodeUI _keyCodeUi;

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

    public void SetImage()
    {
        if (_spellItem != null)
        {
            GetComponent<Image>().sprite = _spellItem._UIsprite;
        }
    }
    
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        selected = true;
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