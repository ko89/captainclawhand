using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    [SerializeField]
    Vector3 EulerAngles;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(EulerAngles);
    }
}
