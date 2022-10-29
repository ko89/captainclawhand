using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"<color=\"yellow\">[OnCollisionEnter]</color> impulse: {collision.impulse}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"<color=\"cyan\">[OnTriggerEnter]</color> contactOffset: {other.contactOffset}");
    }
}
