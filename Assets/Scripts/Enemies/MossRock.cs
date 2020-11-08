using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MossRock : MonoBehaviour
{
    private GameObject leftHand;
    private GameObject rightHand;

    private MeshCollider rockCollider;

    private Damage damageScript;

    private bool isInHands = false;
    private bool isDetatched = false;

    public GameObject targetParticlePrefab;
    public GameObject targetParticleRef;

    public GameObject rockParticlePrefab;

    private Vector3 throwDirection;
    private float velocity = 20.0f;

    private float rotateValue = 1.0f;

    //Screen shake
    private float screenShakeMaxDistance = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        rockCollider = GetComponent<MeshCollider>();
        rockCollider.enabled = false;

        damageScript = GetComponent<Damage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInHands)
        {
            transform.position = (leftHand.transform.position + rightHand.transform.position) / 2.0f;
            transform.rotation = Quaternion.Euler(-90.0f, 0, -90.0f);
        }

        if(isDetatched)
        {
            transform.position += throwDirection * velocity * Time.deltaTime;

            transform.Rotate(new Vector3(rotateValue, rotateValue, rotateValue));
        }
    }

    public void SetHandReferences(GameObject leftHand, GameObject rightHand)
    {
        this.leftHand = leftHand;
        this.rightHand = rightHand;

        isInHands = true;
    }

    public void SetTargetDirection(Vector3 landingPosition)
    {
        throwDirection = (landingPosition - transform.position).normalized;

        GameObject go = GameObject.Instantiate(targetParticlePrefab, landingPosition - new Vector3(0.0f, 0.5f, 0.0f), targetParticlePrefab.transform.rotation) ;
        targetParticleRef = go;
    }

    public void DetatchFromBoss()
    {
        isInHands = false;
        isDetatched = true;

        rockCollider.enabled = true;

        transform.rotation = Quaternion.Euler(90.0f, 0, 90.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) return;

        //Explicit check against Room_Boss_2(Clone) box collider trigger (cannot manipulate Physics, Default layer required for collision with floor/walls
        if (other.gameObject.name == "Room_Boss_2(Clone)") return;

        if (other.gameObject.CompareTag("Player") && other.gameObject.name.Equals("Weapon")) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            damageScript.DealDamage(other);
        }

        float screenShakeAmount = 1.0f - ((GameManager.Instance._player.transform.position - transform.position).magnitude / screenShakeMaxDistance);
        ScreenShakeManager.Instance.ScreenShake(0.25f, screenShakeAmount);

        GameObject.Instantiate(rockParticlePrefab, transform.position, Quaternion.identity);
        AudioManager.PlaySoundAtPosition("MossRockLand", transform.position, 1.0f, Random.Range(0.5f, 0.75f));

        Destroy(targetParticleRef);
        Destroy(gameObject);

    }
}
