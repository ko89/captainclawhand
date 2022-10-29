using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Transform _leftPaddleForcePoint, _rightPaddleForcePoint;


    private void MoveBoatByForce(Transform t, float strength)
    {
        if (t != null && _rigidbody != null)
            _rigidbody.AddForceAtPosition(strength * t.forward, t.position);
    }
    public void HandlePaddleLeft(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();

        Debug.Log($"[HandlePaddleRight] value: {value}");

        MoveBoatByForce(_leftPaddleForcePoint, 1f);
    }

    public void HandlePaddleRight(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();

        Debug.Log($"[HandlePaddleRight] value: {value}");

        MoveBoatByForce(_rightPaddleForcePoint, 1f);
    }


    //private void Foo()
    //{
    //    PlayerInput playerInput;
    //    playerInput.
    //}
}
