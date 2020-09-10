using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Room : MonoBehaviour
{
    public GameObject[] exits;
    public GameObject[] spawnZones;
    public bool isActive = false;
    public bool isCleared = false;
}
