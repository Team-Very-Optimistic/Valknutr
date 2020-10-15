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
    protected TextMeshProUGUI _tooltipText;
    protected GameObject _tooltipObject;
    protected RectTransform _tooltipRectTransform;
    protected string _tooltipString;
    public RectTransform _tooltipPosition;
    

    private void Start()
    {
        _oriParent = transform.parent;
        _oriPos = transform.position;
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_spellItem != null)
        {
            transform.GetChild(1).GetComponent<Image>().sprite = _spellItem._UIsprite;
            _tooltipString = _spellItem._tooltipMessage;
        }

        if (_spellItem.isBaseSpell)
        {
            GetComponent<Mask>().enabled = false;
            //transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
        _tooltipObject = transform.Find("Tooltip").gameObject;
        _tooltipRectTransform = _tooltipObject.GetComponent<RectTransform>();
        _tooltipText = _tooltipObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTooltipString(string tooltipString)
    {
        _tooltipString = tooltipString;
    }
    
    //Detect if the Cursor starts to pass over the GameObject
    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        ShowTooltip(_tooltipString);
    }

    //Detect when Cursor leaves the GameObject
    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        HideTooltip();
    }

    public void Init()
    {
        if (_spellItem != null)
        {
            transform.GetChild(1).GetComponent<Image>().sprite = _spellItem._UIsprite;
            _tooltipString = _spellItem._tooltipMessage;
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

    public void ShowTooltip(string tooltipString)
    {
        _tooltipObject.SetActive(true);
        _tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgoundSize = new Vector2(_tooltipText.preferredWidth + textPaddingSize, _tooltipText.preferredHeight + textPaddingSize);
        _tooltipRectTransform.sizeDelta = backgoundSize;

        if (_tooltipPosition != null)
        {
            Debug.Log("not null");
            _tooltipObject.transform.SetParent(_tooltipPosition);
            _tooltipRectTransform.position = _tooltipPosition.position;
        }
        //_tooltipRectTransform.localPosition = backgoundSize / 2;
    }

    public void HideTooltip()
    {
        _tooltipObject.SetActive(false);
    }
}