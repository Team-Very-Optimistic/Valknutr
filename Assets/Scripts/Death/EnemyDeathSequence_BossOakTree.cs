using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathSequence_BossOakTree : EnemyDeathSequence
{
    public Animator animator;
    public GameObject particleEmitLocation;
    public List<Material> smrMaterials;

    public float particleEmitTime;

    private bool isEmitting;
    private float particleEmitTimeElapsed;

    private new void Start()
    {
        animator = GetComponent<Animator>();

        //Change materials to transparent
        SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            smrMaterials.AddRange(smr.materials);
        }
    }

    // Update is called once per frame
    private new void Update()
    {
        if (isEmitting)
        {
            particleEmitTimeElapsed += Time.deltaTime;

            float percentageFade = particleEmitTimeElapsed / particleEmitTime;

            foreach (Material mat in smrMaterials)
            {
                Color matColor = mat.color;
                matColor.a = 1.0f - 1.0f * percentageFade;
                mat.color = matColor;
            }

            if(percentageFade >= 1.0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //Check until anim end
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                isEmitting = true;
                GameObject particleSystem = Instantiate(deathParticleSystemPrefab, particleEmitLocation.transform.position, Quaternion.identity);
                Destroy(particleSystem, particleEmitTime + 2.0f);

                foreach(Material mat in smrMaterials)
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
        }
    }

    public override void StartDeathSequence()
    {
        GetComponent<EnemyBehaviour_Boss_OakTree>().SetDeathState();
        animator.SetTrigger("ToDeath");
    }
}
