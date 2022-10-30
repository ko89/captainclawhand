using UnityEngine;
using UnityEngine.InputSystem;

public class MultiPlayer : MonoBehaviour
{
    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        var gameObject = playerInput.gameObject;
    }
}
