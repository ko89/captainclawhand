using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    [Header("Physics stuff")]
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Transform _centerPoint;
    [SerializeField]
    private Transform _leftPaddleForcePoint, _rightPaddleForcePoint;

    [Header("Input stuff")]
    [SerializeField]
    private float _accelMultiplier = 200f;
    [SerializeField]
    private float _positionLerpSpeed = 0.9f;
    [SerializeField]
    private Transform _gyroDebug;
    [SerializeField]
    private float _gyroDebugMultiplier = 1f;

    private float _positionOffset = 0f;


    [SerializeField]
    private float _paddleStrengtIncrease = 1f;
    [SerializeField]
    private float _paddleStrengthMax = 5f;
    [SerializeField]
    private AnimationCurve _paddleStrengthCurve;

    private float _triggerLeftValue = 0f, _triggerRightValue = 0f;
    private float _paddleLeftTime = 0f, _paddleRightTime = 0f;

    [Header("Animation stuff")]
    [SerializeField]
    private Animator _playerAnimator;
    [Header("Sound stuff")]
    [SerializeField]
    private AudioSource _playerSource;
    [SerializeField]
    private AudioFXSource _paddleSounds;
    [SerializeField]
    private AudioFXSource _swooshSounds;

    Quaternion _gyroValue = Quaternion.identity;
    Vector3 _gyroValueRaw = Vector3.zero;
    Vector3 _accelValue = Vector3.zero;
    Vector3 _accelValueRaw = Vector3.zero;

    enum Side
    {
        Left,
        Right
    }


    void Start()
    {
        // Gyroscope input callback
        var action = new InputAction(binding: "<Gamepad>/gyro");
        action.performed += HandleGyro;
        action.Enable();

        var actionAccel = new InputAction(binding: "<Gamepad>/accel");
        actionAccel.performed += HandleAccel;
        actionAccel.Enable();
    }

    void FixedUpdate()
    {
        if (_gyroDebug != null)
        {
#if true
            // Current status
            var rot = _gyroDebug.localRotation;

            // Rotation from gyroscope
            rot *= _gyroValue;
            _gyroValue = Quaternion.identity;

            // Accelerometer input
            //var accel = Gamepad.current?.GetChildControl<Vector3Control>("accel");
            //var gravity = accel?.ReadValue() ?? -Vector3.up;
            var gravity = _accelValueRaw;

            // Drift compensation using gravitational acceleration
            var comp = Quaternion.FromToRotation(rot * gravity, -Vector3.up);

            // Compensation reduction
            comp.w *= 0.2f / Time.fixedDeltaTime;
            comp = comp.normalized;

            var localRotation = comp * rot;

            // Update
            _gyroDebug.localRotation = localRotation;

            Vector3 foo = Vector3.up;
            Vector3 rotatedFoo = localRotation * foo;
#endif

#if false
            _positionOffset += _accelValue.x;
            _positionOffset = Mathf.Lerp(0f, _positionOffset, _positionLerpSpeed);

            var localPosition = _gyroDebug.localPosition;
            localPosition.x = _positionOffset;
            _gyroDebug.localPosition = localPosition;
#endif

            //_gyroDebug.transform.position = new Vector3(_gyroValueRaw.y, 0f, 0f) * _gyroDebugMultiplier;
        }




        Paddle(_centerPoint, _leftPaddleForcePoint, _triggerLeftValue, ref _paddleLeftTime, Side.Left);
        Paddle(_centerPoint, _rightPaddleForcePoint, _triggerRightValue, ref _paddleRightTime, Side.Right);
    }



    private void Paddle(Transform centerPoint, Transform forcePoint, float triggerValue, ref float time, Side side)
    {
        if (_paddleStrengthCurve == null || _centerPoint == null || forcePoint == null)
            return;

        if (triggerValue > 0.5f)
        {
            if(time == 0)
            {
                _paddleSounds.PlayOneShot(_playerSource);
            }


            time += Time.fixedDeltaTime;
            var strength = _paddleStrengthCurve.Evaluate(time);
            var forcePosition = Vector3.Lerp(centerPoint.position, forcePoint.position, Mathf.Clamp01(strength) * 0.5f);
            MoveBoatByForce(forcePoint.forward * strength, forcePosition);

            _playerAnimator.SetFloat(side == Side.Left ? "PaddelLinks" : "PaddelRechts", 1.0f);
        }
        else
        {
            time = 0f;

            _playerAnimator.SetFloat(side == Side.Left ? "PaddelLinks" : "PaddelRechts", 0.0f);
        }
    }

    private void MoveBoatByForce(Vector3 direction, Vector3 worldPoint)
    {
        if (_rigidbody != null)
            _rigidbody.AddForceAtPosition(direction, worldPoint);
    }
    public void HandlePaddleLeft(InputAction.CallbackContext context)
    {
        _triggerLeftValue = context.ReadValue<float>();
    }

    public void HandlePaddleRight(InputAction.CallbackContext context)
    {
        _triggerRightValue = context.ReadValue<float>();
    }

    public void HandleGyro(InputAction.CallbackContext context)
    {
        // Gyro input data
        var gyro = context.ReadValue<Vector3>();
        _gyroValueRaw = gyro;

        // Coefficient converting a gyro data value into a degree
        // Note: The actual constant is undocumented and unknown.
        //       I just put a plasible value by guessing.
        const double GyroToAngle = 16 * 360 / System.Math.PI;

        // Delta time from the last event
        var dt = context.time - context.control.device.lastUpdateTime;
        dt = System.Math.Min(dt, 1.0 / 60); // Discarding large deltas

        var gyroRotation = Quaternion.Euler(gyro * (float)(GyroToAngle * dt));

        _gyroValue *= gyroRotation;
    }

    public void HandleAccel(InputAction.CallbackContext context)
    {
        // Gyro input data
        var accelValue = context.ReadValue<Vector3>();
        _accelValueRaw = accelValue;

        // Delta time from the last event
        var dt = context.time - context.control.device.lastUpdateTime;
        dt = System.Math.Min(dt, 1.0 / 60); // Discarding large deltas

        var accel = accelValue * (float)dt;

        _accelValue = _accelMultiplier * accel;
    }
}
