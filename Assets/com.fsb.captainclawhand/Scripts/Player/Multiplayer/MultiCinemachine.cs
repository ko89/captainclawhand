using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiCinemachine : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup _cinemachineTargetGroup;
    [SerializeField]
    private TargetFollowHelper _targetFollowHelper;

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        var gameObject = playerInput.gameObject;

        _cinemachineTargetGroup.AddMember(gameObject.transform, 1f, 1f);

        if (_targetFollowHelper != null)
            _targetFollowHelper.Target = gameObject.transform;
    }
}
