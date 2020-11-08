using System;
using System.Collections;
using UnityEngine;
 
///summary
///summary
public class BossTrigger : MonoBehaviour
{
 
    #region Public Fields

    public GameObject boss;
    public GameObject door;
    #endregion
 
    #region Unity Methods

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance._player)
        {
            StartCoroutine(SpawnBoss());
        }
    }

    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(0.1f);
        boss.SetActive(true);
        door.SetActive(true);
    }

    #endregion
 
   
}