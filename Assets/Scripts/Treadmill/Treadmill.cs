using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
struct DetectJoint
{
    public Transform JointTransform;
    public Quaternion InitialRotation;
    public float Distance;
    public Plane DetectionPlane;
    public float Width;
}

public class Treadmill : MonoBehaviour
{
    //public LevelDefinition LevelDefinition;
    public GameObject Level;
    public Vector3 OffsetRotation = new(0, 0, 0);
    public float Radius = 1500;
    public Transform CharacterCenter;
    public float DetectionDistance;
    //public float DetectionWidth; 
    public AnimationCurve DetectionBlend;



    [SerializeField]
    private List<DetectJoint> Joints = new();
    // Start is called before the first frame update
    void Start()
    {
        // Expect Tileformat Tile-0-1 etc.
        var tileSets = Level.GetComponentsInChildren<Transform>()
            .Select(t => t.gameObject)
            .Where(t => t.name.Contains("Tile"))
            .Select(t => new { go = t, elements = t.name.Split("-") })
            .OrderBy(t => int.Parse(t.elements[2]))
            .GroupBy(t => t.elements[1]);

        // Create Bone Link Structure
        foreach (var tileSet in tileSets)
        {
            Transform prevGo = null;
            Plane previousPlane = new(Vector3.forward, 0);

            foreach (var tile in tileSet)
            {
                GameObject tileGO = tile.go;
                if(!prevGo)
                {
                    prevGo = tileGO.transform;
                    continue;
                }

                tileGO.transform.SetParent(prevGo);

                var initialRotation = tileGO.transform.localRotation;
                var blendRotation = initialRotation;

                Plane detectionPlane = new(tileGO.transform.up, tileGO.transform.position);
                float width = detectionPlane.distance - previousPlane.distance;
                Joints.Add(
                    new DetectJoint
                    {
                        JointTransform = tileGO.transform,
                        InitialRotation = initialRotation,
                        DetectionPlane = previousPlane,
                        Width = width
                    }
                );

                prevGo = tileGO.transform;
                previousPlane = detectionPlane;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var targetPosition = CharacterCenter.position;

        foreach (DetectJoint joint in Joints)
        {
            Quaternion initialRotation = joint.InitialRotation;
            var offetQuaternion = Quaternion.Euler(-15.0f, 0, 0);
            Quaternion blendTransform = offetQuaternion * joint.InitialRotation;
   

            Plane detectionPlane = joint.DetectionPlane;
            float distanceToJoint = detectionPlane.GetDistanceToPoint(targetPosition);

            if (distanceToJoint < 0)
                Debug.DrawLine(joint.JointTransform.position, joint.JointTransform.position + new Vector3(0, 0, 10), Color.red);
            else
                Debug.DrawLine(joint.JointTransform.position, joint.JointTransform.position + new Vector3(0, 0, 10), Color.blue);

            float blendWeight = DetectionBlend.Evaluate(distanceToJoint);
            Quaternion resultRotation = Quaternion.Slerp(initialRotation, blendTransform, blendWeight);

            joint.JointTransform.localRotation = resultRotation;
            DetectionBlend.Evaluate(distanceToJoint);
            Debug.Log(distanceToJoint);
        }
    }
}
