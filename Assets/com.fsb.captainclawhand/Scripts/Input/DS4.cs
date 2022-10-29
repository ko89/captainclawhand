// https://blog.sg4yk.com/dual_shock_motion_in_unity_en.html

using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class DS4
{
    // Gyroscope
    public static ButtonControl gyroX = null;
    public static ButtonControl gyroY = null;
    public static ButtonControl gyroZ = null;

    public static ButtonControl accelX = null;
    public static ButtonControl accelY = null;
    public static ButtonControl accelZ = null;

    public static Gamepad controller = null;

    public static Gamepad getConroller(string layoutFile = null)
    {
        // Read layout from JSON file
        string layout = File.ReadAllText(layoutFile == null ? "resources/input/customcontrol.json" : layoutFile);

        // Overwrite the default layout
        InputSystem.RegisterLayoutOverride(layout, "DualShock4GamepadHIDCustom");

        var ds4 = Gamepad.current;
        DS4.controller = ds4;
        bindControls(DS4.controller);
        return DS4.controller;
    }

    private static void bindControls(Gamepad ds4)
    {
        gyroX = ds4.GetChildControl<ButtonControl>("gyro X 14");
        gyroY = ds4.GetChildControl<ButtonControl>("gyro Y 16");
        gyroZ = ds4.GetChildControl<ButtonControl>("gyro Z 18");

        accelX = ds4.GetChildControl<ButtonControl>("accel X 20");
        accelY = ds4.GetChildControl<ButtonControl>("accel Y 22");
        accelZ = ds4.GetChildControl<ButtonControl>("accel Z 24");
    }

    public static Quaternion getRotation(float scale = 1)
    {
        float x = processRawData(gyroX.ReadValue()) * scale;
        float y = processRawData(gyroY.ReadValue()) * scale;
        float z = -processRawData(gyroZ.ReadValue()) * scale;
        return Quaternion.Euler(x, y, z);
    }

    public static Vector3 getAccelration(float scale = 1)
    {
        float x = processRawData(accelX.ReadValue()) * scale;
        float y = processRawData(accelY.ReadValue()) * scale;
        float z = -processRawData(accelZ.ReadValue()) * scale;

        return new Vector3(x, y, z);
    }

    private static float processRawData(float data)
    {
        return data > 0.5 ? 1 - data : -data;
    }
}