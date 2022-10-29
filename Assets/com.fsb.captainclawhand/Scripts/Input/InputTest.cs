using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    private Gamepad controller = null;

    [SerializeField]
    private float _accelMultiplier = 1000f;

    private float _positionOffset = 0f;

    [SerializeField]
    private float _positionLerpSpeed = 0.5f;

    void Start()
    {
        this.controller = DS4.getConroller();
    }

    void Update()
    {
        if (controller == null)
        {
            try
            {
                controller = DS4.getConroller();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            // Press circle button to reset rotation
            if (controller.buttonEast.isPressed)
            {
                transform.rotation = Quaternion.identity;
                Debug.Log("asd");
            }
            transform.rotation *= DS4.getRotation(4000 * Time.deltaTime);


            var accel = DS4.getAccelration(_accelMultiplier * Time.deltaTime);
            _positionOffset += accel.x;
            _positionOffset = Mathf.Lerp(0f, _positionOffset, _positionLerpSpeed);

            var localPosition = transform.localPosition;
            localPosition.x = _positionOffset;
            transform.localPosition = localPosition;
        }
    }
}