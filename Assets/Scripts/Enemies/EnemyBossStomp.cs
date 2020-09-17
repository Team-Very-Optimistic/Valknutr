using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossStomp : MonoBehaviour
{
    public float expireTime;
    private float elapsedTime = 0.0f;
    private float maxScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, expireTime);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        float percent = (elapsedTime / expireTime);
        transform.localScale = new Vector3(percent * maxScale, this.transform.localScale.y, percent * maxScale);

        Color currentColor = this.GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f - (percent * 1.0f));
    }

    public void SetMaxScale(float scale)
    {
        maxScale = scale;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            var damageScript = GetComponent<Damage>();
            damageScript.DealDamage(other);
        }
    }
}
