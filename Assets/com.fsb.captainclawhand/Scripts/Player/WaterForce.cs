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
    private float _levelStrength = 1f;
    private Vector3 _levelUpReference = new Vector3(0f, 1f, 0f);

    private void Update()
    {
        if (_rigidbody == null)
        {
            enabled = false;
            return;
        }

        float depth = _rigidbody.transform.position.y - _upForceDepthTrigger;
        if (depth < 0f)
        {
            float multiplier = Mathf.Pow(depth, 2f);
            _rigidbody.AddForce(_upForce * Time.deltaTime * _upForceStrength * multiplier, ForceMode.Force);
            
            //Velocity-based drag (reduces y speed when under-water)
            float downSpeed = _rigidbody.velocity.y;
            _rigidbody.AddForce(new Vector3(0, -downSpeed * 0.2f, 0), ForceMode.VelocityChange);
        }


        var up = _rigidbody.transform.up;
        if (Mathf.Abs(up.x) > 0.1f || Mathf.Abs(up.z) > 0.1f)
        {
            var upPlane = new Vector3(up.x, 0f, up.z);
            var upPlaneNormalized = upPlane.normalized;

            var forcePosition = _rigidbody.transform.position + upPlaneNormalized;

            float levelStrength = Vector3.Dot(_levelUpReference, up);
            levelStrength = 1f - ((levelStrength + 1) * 0.5f);

            var upCenter = _rigidbody.transform.position + _levelUpReference;
            var forceDirection = upCenter - forcePosition;

            Debug.Log($"<color=\"green\">levelStrength: {levelStrength}</color>");

            _rigidbody.AddForceAtPosition(forceDirection * levelStrength * Time.deltaTime * _levelStrength, forcePosition);
        }


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
}
