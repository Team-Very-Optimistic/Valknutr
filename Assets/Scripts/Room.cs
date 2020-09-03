using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Room : MonoBehaviour
{
    public GameObject[] exits;

    private void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
    }

    private void OnTriggerStay(Collider other)
    {
        print("trigger stay");
    }
}
