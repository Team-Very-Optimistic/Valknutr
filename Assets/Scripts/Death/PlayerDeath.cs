using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private void OnDestroy()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
    }
}
