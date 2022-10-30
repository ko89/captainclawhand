using UnityEngine;
using UnityEngine.InputSystem;

public class InjectCustomLayout : MonoBehaviour
{
    const string LayoutJson = @"{
      ""name"": ""DualShock4GamepadHIDCustom"",
      ""extend"": ""DualShock4GamepadHID"",
      ""controls"": [
        {""name"":""gyro"", ""format"":""VC3S"", ""offset"":13, ""layout"":""Vector3"", ""processors"":""ScaleVector3(x=-1,y=-1,z=1)""},
        {""name"":""gyro/x"", ""format"":""SHRT"", ""offset"":0 },
        {""name"":""gyro/y"", ""format"":""SHRT"", ""offset"":2 },
        {""name"":""gyro/z"", ""format"":""SHRT"", ""offset"":4 },
        {""name"":""accel"", ""format"":""VC3S"", ""offset"":19, ""layout"":""Vector3"", ""processors"":""ScaleVector3(x=-1,y=-1,z=1)""},
        {""name"":""accel/x"", ""format"":""SHRT"", ""offset"":0 },
        {""name"":""accel/y"", ""format"":""SHRT"", ""offset"":2 },
        {""name"":""accel/z"", ""format"":""SHRT"", ""offset"":4 }
      ]}";

    [RuntimeInitializeOnLoadMethod]
    private static void InjectDS4GyroAccel()
    {
        InputSystem.RegisterLayoutOverride(LayoutJson);

        Debug.Log($"<color=\"grey\">[InjectCustomLayout.InjectDS4GyroAccel]</color>");
    }
}
