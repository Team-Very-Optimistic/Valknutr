using System;
using UnityEngine;
 
///summary
///summary
public class SecretRoomEntrance : HealthScript
{
    public GameObject rooms;
    public GameObject door;
    public override void OnDeath()
    {
        rooms.SetActive(true);
        door.SetActive(false);
        base.OnDeath();
    }

    public override void Start()
    {
        base.Start();
    }

    public void OnEnable()
    {
        rooms.SetActive(false);
    }
}