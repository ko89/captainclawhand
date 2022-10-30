using UnityEngine;

// follow target without changing rotation
public class TargetFollowHelper : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Vector3 _worldOffset = Vector3.zero;

    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }

    private void Update()
    {
        if (_target != null)
            transform.position = _target.position + _worldOffset;
    }
}
