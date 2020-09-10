using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private GameObject parent;
    private Vector3 lastKnownPos;
    private Vector3 yOffset = new Vector3(0.0f, 1.0f, 0.0f);
    private float timeElapsed = 0.0f;

    [SerializeField]
    private float aliveTime;

    void Start()
    {
        Destroy(gameObject, aliveTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        //0 - Mathf.Pi
        float proportion = timeElapsed / aliveTime * Mathf.PI;

        if(parent != null)
        {
            lastKnownPos = parent.transform.position;
        }

        this.transform.position = lastKnownPos + yOffset + new Vector3(proportion / 2.0f, Mathf.Sin(proportion) * 1.5f, 0.0f);      
    }

    public void SetDamageTextProperties(float damage, Quaternion rotation, GameObject parent)
    {
        this.GetComponent<TextMesh>().text = damage.ToString();
        transform.rotation = rotation;
        this.parent = parent;
        lastKnownPos = parent.transform.position;
    }
}
