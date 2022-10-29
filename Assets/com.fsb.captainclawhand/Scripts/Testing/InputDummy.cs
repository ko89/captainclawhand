using UnityEngine;
using UnityEngine.InputSystem;

public class InputDummy : MonoBehaviour
{
    void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            Debug.Log("asd");
        }

        Vector2 move = gamepad.leftStick.ReadValue();

        Debug.Log($"move: {move}");
    }
}