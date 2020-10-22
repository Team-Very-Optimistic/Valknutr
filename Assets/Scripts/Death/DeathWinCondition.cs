using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWinCondition : MonoBehaviour
{
    public void OnDeath()
    {
        GameManager.Instance.SetGameWin();
    }
}
