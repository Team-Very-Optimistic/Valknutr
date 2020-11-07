using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathSequence : BaseDeathSequence
{
    public GameObject deathParticleSystemPrefab;
    public List<Material> ragdollMaterials;

    public float delayBeforeFade;
    public float timeToDestroy;
    private float timeToFade;
    private float timeElapsedToFade;

    public bool triggeredParticle = false;
    public bool triggeredDeathSequence = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timeToFade = timeToDestroy - delayBeforeFade;

        SkinnedMeshRenderer[] skinnedMeshRenderers = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            ragdollMaterials.AddRange(smr.materials);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {  
        if(triggeredDeathSequence)
        {
            delayBeforeFade -= Time.deltaTime;

            if (delayBeforeFade <= 0.0f)
            {
                if (!triggeredParticle)
                {
                    GameObject particleSystem = Instantiate(deathParticleSystemPrefab, transform.GetChild(0).transform.position, Quaternion.identity);
                    particleSystem.GetComponent<DeathParticleSystem>().SetRagdollAliveTime(timeToFade);
                    particleSystem.GetComponent<DeathParticleSystem>().SetRagdollChildGameObject(base.ragdollParts[0].gameObject);
                    triggeredParticle = true;

                    foreach (Material mat in ragdollMaterials)
                    {
                        //Set material to "Fade" to perform full transparency
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.EnableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                }

                timeElapsedToFade += Time.deltaTime;

                float percentageFade = timeElapsedToFade / timeToFade;

                foreach (Material mat in ragdollMaterials)
                {
                    Color matColor = mat.color;
                    matColor.a = 1.0f - 1.0f * percentageFade;
                    mat.color = matColor;
                }
            }
        }
    }

    public virtual void StartDeathSequence()
    {
        TriggerRagdoll();
        KnockbackRagdoll();
        CallDestroy(timeToDestroy);
        triggeredDeathSequence = true;
        GetComponent<NavMeshAgent>().speed = 0.0f;

        Destroy(GetComponent<EnemyBehaviourBase>());

        if (GetComponent<DropsLoot>() != null)
        {
            GetComponent<DropsLoot>().OnDeath();
        }

        if(GetComponent<EnemyBehaviour_Boss>() != null)
        {
            GetComponent<EnemyBehaviour_Boss>().SetDeathState();
        }

        Destroy(GetComponent<EnemyShielder_Link>());
    }
}
