using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MossRock : MonoBehaviour
{
    private GameObject leftHand;
    private GameObject rightHand;

    private BoxCollider rockCollider;

    private Damage damageScript;

    private bool isInHands = false;
    private bool isDetatched = false;

    public GameObject targetParticlePrefab;
    public GameObject targetParticleRef;

    // Slerp
    private Vector3 oppLandingPosition;
    private Vector3 throwPosition;
    private Vector3 landingPosition;
    private Vector3 riseRelCenter;
    private Vector3 setRelCenter;
    private float startTime;
    public float rockJourneyTime = 2.0f;

    //Screen shake
    private float screenShakeMaxDistance = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        rockCollider = GetComponent<BoxCollider>();
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
            transform.rotation = Quaternion.Euler(90.0f, 0, 90.0f);

            float fracComplete = 0.5f + ((Time.time - startTime) / rockJourneyTime);

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += throwPosition;
            transform.position = new Vector3(transform.position.x, Mathf.Abs(transform.position.y), transform.position.z);

            if(fracComplete >= 0.95f)
            {
                Destroy(targetParticleRef);
                Destroy(gameObject);
            }
        }
    }

    public void SetHandReferences(GameObject leftHand, GameObject rightHand)
    {
        this.leftHand = leftHand;
        this.rightHand = rightHand;

        isInHands = true;
    }

    public void SetTargetPosition(Vector3 landingPosition)
    {
        throwPosition = this.transform.position;
        this.landingPosition = landingPosition;

        //Get opposite position for slerp
        Vector3 horizontalDirection = new Vector3(throwPosition.x - landingPosition.x, 0.0f, throwPosition.z - landingPosition.z);
        oppLandingPosition = landingPosition + 2.0f * horizontalDirection;

        riseRelCenter = oppLandingPosition - throwPosition;
        setRelCenter = landingPosition - throwPosition;

        startTime = Time.time;

        GameObject go = GameObject.Instantiate(targetParticlePrefab, landingPosition - new Vector3(0.0f, 0.5f, 0.0f), targetParticlePrefab.transform.rotation) ;
        targetParticleRef = go;
    }

    public void DetatchFromBoss()
    {
        isInHands = false;
        isDetatched = true;

        rockCollider.enabled = true;
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
        Destroy(targetParticleRef);
        Destroy(gameObject);
    }
}
