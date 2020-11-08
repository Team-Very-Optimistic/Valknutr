using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemFireOnce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
