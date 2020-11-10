using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSequenceManager : MonoBehaviour
{
    public static SpawnSequenceManager Instance;

    float spawnTime = 3.0f;

    public GameObject spawnParticles;
    public GameObject rockParticles;

    private void Start()
    {
        SpawnSequenceManager.Instance = this;
    }

    public float GetSpawnTime()
    {
        return spawnTime;
    }

    public void SpawnParticlesAtLocation(Vector3 location)
    {
        StartCoroutine(SpawnParticlesAtLocationCoroutine(location));
    }

    public IEnumerator SpawnParticlesAtLocationCoroutine(Vector3 location)
    {
        AudioManager.PlaySound("SpawnBuildup", 0.6f, Random.Range(0.75f, 1.20f));

        Quaternion rotation = new Quaternion(-90.0f, 0.0f, 0.0f, 1.0f);
        GameObject go = Instantiate(spawnParticles, location, rotation);
        go.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        yield return new WaitForSeconds(spawnTime);

        AudioManager.PlaySound("SpawnSound", 0.6f, Random.Range(0.75f, 1.20f));

        go = Instantiate(rockParticles, location, rotation);
        go.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
    }

    public void StartScreenShakeOnSpawn()
    {
        StartCoroutine(StartScreenShakeOnSpawnCoroutine());
    }

    private IEnumerator StartScreenShakeOnSpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);
        ScreenShakeManager.Instance.ScreenShake(0.5f, 0.3f);
    }
}
