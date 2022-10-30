using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterForce : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _upForceDepthTrigger = 0f;
    [SerializeField]
    private float _upForceStrength = 450f;
    private Vector3 _upForce = Vector3.up;

    

    [SerializeField]
    private float _downspeedAntiForce = 0.2f;

    [Header("V2")]
    [SerializeField]
    private float _levelStrength = 1f;
    private Vector3 _levelUpReference = new Vector3(0f, 1f, 0f);


    [Header("v3")]
    [SerializeField]
    private float _dampenFactor = 0.8f;
    [SerializeField]
    private float _adjustFactor = 0.5f;

    private void FixedUpdate()
    {
        if (_rigidbody == null)
        {
            enabled = false;
            return;
        }

        float depth = _rigidbody.transform.position.y - _upForceDepthTrigger;
        if (depth < 0f)
        {
            if (_upForceStrength > 0f)
            {
                float multiplier = Mathf.Pow(depth, 2f);
                _rigidbody.AddForce(_upForce * Time.fixedDeltaTime * _upForceStrength * multiplier, ForceMode.Force);
            }

            if (_downspeedAntiForce > 0f)
            {
                //Velocity-based drag (reduces y speed when under-water)
                float downSpeed = _rigidbody.velocity.y;
                _rigidbody.AddForce(new Vector3(0, -downSpeed * _downspeedAntiForce * Time.fixedDeltaTime, 0), ForceMode.VelocityChange);
            }
        }


        var up = _rigidbody.transform.up;
        /*if (Mathf.Abs(up.x) > 0.1f || Mathf.Abs(up.z) > 0.1f)
        {
            var upPlane = new Vector3(up.x, 0f, up.z);
            var upPlaneNormalized = upPlane.normalized;

            var forcePosition = _rigidbody.transform.position + upPlaneNormalized;

            float levelStrength = Vector3.Dot(_levelUpReference, up);
            levelStrength = 1f - ((levelStrength + 1) * 0.5f);

            var upCenter = _rigidbody.transform.position + _levelUpReference;
            var forceDirection = upCenter - forcePosition;

            //Debug.Log($"<color=\"green\">levelStrength: {levelStrength}</color>");

            _rigidbody.AddForceAtPosition(forceDirection * levelStrength * Time.fixedDeltaTime * _levelStrength, forcePosition);
        }*/

        // Rotate up Torque

        Quaternion deltaQuat = Quaternion.FromToRotation(_rigidbody.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);
        _rigidbody.AddTorque(-_rigidbody.angularVelocity * _dampenFactor, ForceMode.Acceleration);
        _rigidbody.AddTorque(axis.normalized * angle * _adjustFactor, ForceMode.Acceleration);



        /*var upPlane = new Vector3(up.x, 0f, up.z);
        var upPlaneNormalized = upPlane.normalized;

        var forcePosition = _rigidbody.transform.position + upPlaneNormalized;

        float levelStrength = Vector3.Dot(_levelUpReference, up);
        levelStrength = 1f - ((levelStrength + 1) * 0.5f);

        var upCenter = _rigidbody.transform.position + _levelUpReference;
        var forceDirection = upCenter - forcePosition;

        Debug.Log($"<color=\"green\">levelStrength: {levelStrength}</color>");

        _rigidbody.AddForceAtPosition(forceDirection * levelStrength * Time.fixedDeltaTime * _levelStrength, forcePosition);*/


        //var upCenter = _rigidbody.transform.position + _levelUpReference;
        //var upOffCenter = _rigidbody.transform.position + _rigidbody.transform.up;
        //var levelTorque = upCenter - upOffCenter;
        //Debug.Log($"levelToque: {levelTorque.sqrMagnitude}");
        //if (levelTorque.sqrMagnitude > 0.1f)
        //{
        //    //_rigidbody.AddTorque(-levelTorque * _levelStrength * Time.deltaTime);
        //    //Debug.Log($"<color=\"green\">levelTorque: {levelTorque}</color>");

        //    var upOffCenterPlane = new Vector3(upOffCenter.x, 0f, upOffCenter.z);
        //}
    }

    public Vector3 ComputeTorque(Quaternion desiredRotation)
    {
        //q will rotate from our current rotation to desired rotation
        Quaternion q = desiredRotation * Quaternion.Inverse(transform.rotation);
        //convert to angle axis representation so we can do math with angular velocity
        Vector3 x;
        float xMag;
        q.ToAngleAxis(out xMag, out x);
        x.Normalize();
        //w is the angular velocity we need to achieve
        Vector3 w = x * xMag * Mathf.Deg2Rad / Time.fixedDeltaTime;
        w -= _rigidbody.angularVelocity;
        //to multiply with inertia tensor local then rotationTensor coords
        Vector3 wl = transform.InverseTransformDirection(w);
        Vector3 Tl;
        Vector3 wll = wl;
        wll = _rigidbody.inertiaTensorRotation * wll;
        wll.Scale(_rigidbody.inertiaTensor);
        Tl = Quaternion.Inverse(_rigidbody.inertiaTensorRotation) * wll;
        Vector3 T = transform.TransformDirection(Tl);
        return T;
    }
}
