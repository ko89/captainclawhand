using UnityEngine;

// follow target without changing rotation
public class TargetFollowHelper : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Vector3 _worldOffset = Vector3.zero;

    private void Update()
    {
        transform.position = _target.position + _worldOffset;
    }
}
