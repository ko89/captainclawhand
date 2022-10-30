using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
struct JointObject
{
    public Transform Joint;

    public Vector3 FrontPosition;
    public Vector3 InitialPosition;
    public Quaternion InitialRotation;
}

public class TreadmillV2 : MonoBehaviour
{
    //public LevelDefinition LevelDefinition;
    public GameObject Level;
    public float Radius = 1500;
    public Transform CharacterCenter;
    //public float DetectionWidth; 
    public AnimationCurve DetectionBlend;

    //
    private List<JointObject> Joints;
    // Start is called before the first frame update
    void Start()
    {

        List<Transform> children = new();
        for (int i = 0; i < Level.transform.childCount; i++)
        {
            children.Add(Level.transform.GetChild(i));
        }

        Joints = children.Select((child) =>
        {
            GameObject joint = child.gameObject;

            //const bb = child.GetComponents<Renderer>().Aggregate(new Bounds(), (bounds, renderer) => { bounds.Encapsulate(renderer.bounds); return bounds; });



            return new JointObject
            {
                Joint = child,
                InitialPosition = joint.transform.position,
                InitialRotation = joint.transform.rotation,
            };
        }).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //var targetPosition = CharacterCenter.position;
        //var offetQuaternion = Quaternion.Euler(OffsetRotation);

        float characterOffset = CharacterCenter.position.z;
        foreach (JointObject joint in Joints)
        {
            Quaternion initialRotation = joint.InitialRotation;
            Vector3 initialPosition = joint.InitialPosition;

            joint.Joint.SetPositionAndRotation(initialPosition, initialRotation);
            //float rad = (initialPosition.z / Radius) * Mathf.PI;
            //if (initialPosition.z > characterOffset)
            //    continue;
            joint.Joint.transform.Translate(new Vector3(0, 0, characterOffset), Space.World);
            joint.Joint.transform.position += new Vector3(0, 0, -initialPosition.z);
            joint.Joint.RotateAround(new Vector3(0, -Radius, 0), new Vector3(1, 0, 0), 360.0f * (initialPosition.z) / (2.0f * Mathf.PI * Radius));
            joint.Joint.transform.Translate(new Vector3(0, 0, -characterOffset), Space.World);
        }
    }
}
