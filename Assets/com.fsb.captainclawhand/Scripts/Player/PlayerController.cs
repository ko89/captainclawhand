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
    private Transform _gyroDebug;
    [SerializeField]
    private float _gyroDebugMultiplier = 1f;

    
    private Quaternion _gyroValue = Quaternion.identity;
    private Vector3 _gyroValueRaw = Vector3.zero;
    private Vector3 _accelValue = Vector3.zero;
    private Vector3 _accelValueRaw = Vector3.zero;
    private bool _attackActive;

    [Header("Paddle stuff")]
    [SerializeField]
    private AnimationCurve _paddleStrengthCurve;

    private float _triggerLeftValue = 0f, _triggerRightValue = 0f;
    private float _paddleLeftTime = 0f, _paddleRightTime = 0f;

    [Header("Attack stuff")]
    [SerializeField]
    private Transform _attackTargetPosition;
    [SerializeField]
    private float _attackInputPositionLerpSpeed = 0.9f;
    private float _attackInputPosition = 0f;
    [SerializeField]
    private Rigidbody _attackRigidBody;

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
    [SerializeField]
    private AudioFXSource _waveSounds;


    public PlayerData _playerData;


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

    private void Update()
    {
        if (_attackActive)
        {
            _attackInputPosition += _accelValue.x;
            _attackInputPosition = Mathf.Lerp(0f, _attackInputPosition, _attackInputPositionLerpSpeed);

            if (_attackTargetPosition != null)
            {
                var localPosition = _attackTargetPosition.localPosition;
                localPosition.x = _attackInputPosition;
                _attackTargetPosition.localPosition = localPosition;
            }
        }
        _playerAnimator.SetFloat("LeanX", _attackTargetPosition.localPosition.x);
        _playerAnimator.SetFloat("LeanZ", _attackTargetPosition.localPosition.z);
    }

    private void FixedUpdate()
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

        CheckSwing(_attackRigidBody, _attackActive);
    
        int randomWave = Random.Range(0, 90);
        if (randomWave == 0)
            _waveSounds.PlayOneShot(_playerSource);

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

    private Vector3 prevSwingPosition = Vector3.zero;
    private void CheckSwing(Rigidbody swingBody, bool attackActiv)
    {

        if (prevSwingPosition == Vector3.zero)
        {
            prevSwingPosition = swingBody.position;
            return;
        }

        float delta = Vector3.Distance(prevSwingPosition, swingBody.position);

        if(delta > 0.1 && attackActiv)
        {
            _swooshSounds.PlayOneShot(_playerSource);
        }

        prevSwingPosition = swingBody.position;
        //Debug.Log(delta);
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

    public void HandleAttackButton(InputAction.CallbackContext context)
    {
        _attackActive = context.ReadValue<float>() > 0.1f;

        if (_attackTargetPosition != null)
            _attackTargetPosition.gameObject.SetActive(_attackActive);

        _playerAnimator.SetFloat("Attack", _attackActive ? 1.0f : 0.0f);
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
