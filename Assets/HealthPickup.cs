using System;
using UnityEngine;
 
///summary
///summary
public class HealthPickup : MonoBehaviour
{
 
    #region Public Fields

    #endregion
 
    #region Unity Methods



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance._player)
        {
            GameManager.Instance.IncreasePlayerHealth();
        }
    }

    #endregion
 
    #region Private Methods
    #endregion
}