using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float statsMultiplier = 1;

    public virtual void ScaleStats(float newMultiplier)
    {
        var multiplier = newMultiplier / statsMultiplier;
        // var modelScale = Mathf.Log(0.1f + newDifficulty/1.2f) * 0.2f + 1f;
        GetComponent<HealthScript>()?.Scale(multiplier);
        GetComponent<Damage>()?.Scale(multiplier);
        // transform.localScale = Vector3.one * modelScale;

        statsMultiplier = newMultiplier;
    }
}
