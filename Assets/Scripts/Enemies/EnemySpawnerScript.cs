using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject paladinPrefab;
    public GameObject spawnLocationObject;

    //Rotation to spawn prefabs
    private Quaternion prefabRotation;

    //Timing
    public float intervalTimeSpawn;
    private float timeCounter = 0.0f;

    void Start()
    {
        prefabRotation = Quaternion.LookRotation(spawnLocationObject.transform.position - this.gameObject.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;

        if(timeCounter >= intervalTimeSpawn)
        {
            timeCounter -= intervalTimeSpawn;
            GameObject.Instantiate(paladinPrefab, spawnLocationObject.transform.position, prefabRotation);
        }
    }
}
