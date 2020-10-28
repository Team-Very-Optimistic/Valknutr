using UnityEngine;
using System.Collections;
using System;
 
public class Cheats : MonoBehaviour
{
    // Activate corner area size by screen width percentage
    public float ActivateAreaSize = 0.1f;
    
    // How many clicks the player should do before cheats list will be visible
    public int ClicksCount = 5;
    
    // How many seconds player have to click/touch the screen
    public float WaitTime = 2;
    
    private float[] _clickTimes;
    
    private int _clickTimesIndex;
    
    private bool _active = false;
    
    void Start()
    {
        // create clicks array and reset it with float.MinValue
        _clickTimes = new float[ClicksCount];
        ResetClicks();
    }
    
    private void ResetClicks()
    {
        for (int i = 0; i < ClicksCount; i++)
        {
            _clickTimes[i] = float.MinValue;
        }
    }
    
    void Update()
    {
        // check for click or touch and register it
        if (CheckClickOrTouch())
        {
            // click will be registered at time since level load
            _clickTimes[_clickTimesIndex] = Time.timeSinceLevelLoad;
            // each next click will be written on next array index or 0 if overflow
            _clickTimesIndex = (_clickTimesIndex + 1) % ClicksCount;
        }
        
        // check if cheat list should be activated
        if (ShouldActivate())
        {
            _active = true;
            ResetClicks();
        }
    }
    
    // checks if cheat list should be activated
    private bool ShouldActivate()
    {
        // check if all click/touches were made within WaitTime
        foreach(float clickTime in _clickTimes)
        {
            if (clickTime < Time.timeSinceLevelLoad - WaitTime)
            {
                // return false if any of click/touch times has been done earlier
                return false;
            }
        }
        
        // if we are here, cheat should be activated
        return true;
    }
    
    // returns true if there's click or touch within the activate area
    private bool CheckClickOrTouch()
    {
        // convert activation area to pixels
        float sizeInPixels = ActivateAreaSize * Screen.width;
        
        // get the click/touch position
        Vector2? position = ClickOrTouchPoint();
        
        if (position.HasValue) // position.HasValue returns true if there is a click or touch
        {
            // check if withing the range
            if (position.Value.x >= Screen.width - sizeInPixels && Screen.height - position.Value.y <= sizeInPixels)
            {
                return true;
            }
        }
        
        return false;
    }
    
    // checks for click or touch and returns the screen position in pixels
    private Vector2? ClickOrTouchPoint()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse click
        {
            return Input.mousePosition;
        } else if (Input.touchCount > 0) // one or more touch
        {
            // check only the first touch
            Touch touch = Input.touches[0];
            
            // it should react only when the touch has just began
            if (touch.phase == TouchPhase.Began) {
                return touch.position;
            }
        }
        
        // null if there's no click or touch
        return null;
    }
    
    void OnGUI()
    {
        if (_active)
        {
            // display cheats list here...
            DisplayCheat("Close Cheat Menu", () => _active = false);
            //DisplayCheat("Test Cheat 1", () => Debug.Log("Test cheat Activated!"));
            DisplayCheat("Inverse Gravity", () => Physics.gravity = -Physics.gravity);
            DisplayCheat("Unlock all spells", () => UnlockAllSpells());
            DisplayCheat("GodMode", () => GodMode());
            DisplayCheat("Spawn Item", () => GameManager.Instance.SpawnItem(Util.GetMousePositionOnWorldPlane(Camera.main)));
            DisplayCheat("Spawn Chest (Q=1)", () => GameManager.SpawnTreasureChest(Util.GetMousePositionOnWorldPlane(Camera.main), 1));
            DisplayCheat("Spawn Chest (Q=10)", () => GameManager.SpawnTreasureChest(Util.GetMousePositionOnWorldPlane(Camera.main), 10));
            DisplayCheat("Spawn Chest (Q=100)", () => GameManager.SpawnTreasureChest(Util.GetMousePositionOnWorldPlane(Camera.main), 100));
            DisplayCheat("Spawn Chest (Q=1000)", () => GameManager.SpawnTreasureChest(Util.GetMousePositionOnWorldPlane(Camera.main), 1000));
        }
    }

    private void UnlockAllSpells()
    {
        foreach (var item in GameManager.Instance._itemList._SpellItems)
        {
            var v = Instantiate(item);
            
            if (v.isBaseSpell)
            {
                var copy = (SpellBase) Instantiate(v._spellElement);
                copy.InitCopy();
                v._spellElement = copy;
            }
            else
            {
                SpellModifier mod = (SpellModifier) Instantiate(v._spellElement);
                mod.UseValue();
                v._spellElement = mod;
            }
            Inventory.Instance.Add(v);
        }
        
    }

    private void GodMode()
    {
        GameManager.Instance._player.GetComponent<PlayerHealth>().AddHealth(99999);
    }
    
    private void DisplayCheat(string cheatName, Action clickedCallback)
    {
        if (GUILayout.Button("Cheat: " + cheatName))
        {
            clickedCallback();
        }
    
    }
}
    
