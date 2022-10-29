using UnityEngine;

// follow target without changing rotation
public class TargetFollowHelper : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private void Update()
    {
        transform.position = _target.position;
    }
}
