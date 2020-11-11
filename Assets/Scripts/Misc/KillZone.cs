using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KillZone : MonoBehaviour
{
    public float distanceCutoff;
    private void OnTriggerEnter(Collider other)
    {
        var activeRoom = GameManager.Instance.activeRoom.gameObject.transform.position;
        if (Vector3.Distance(activeRoom, other.ClosestPoint(activeRoom)) > distanceCutoff)
        {
            Debug.LogWarning("KillZone kills : " + other.name, other);
            Destroy(other);
        }
        else
        {
            other.transform.position = activeRoom + 2 * (Random.insideUnitSphere + Vector3.up);
        }
    }
}
