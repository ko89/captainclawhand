using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioFXSource CrashSounds;


    private void OnCollisionEnter(Collision collision)
    {
        CrashSounds.PlayOneShot(AudioSource);
        collision.gameObject.GetComponent<PlayerController>()._playerData.Health -= 10;
        //Debug.Log($"<color=\"yellow\">[OnCollisionEnter]</color> name: {name} impulse: {collision.impulse}");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"<color=\"cyan\">[OnTriggerEnter]</color> name: {name} contactOffset: {other.contactOffset}");
    }
}
