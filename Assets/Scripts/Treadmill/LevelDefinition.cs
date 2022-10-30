using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SliceTile
{
    public GameObject Prefab;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelDefinition", order = 1)]
public class LevelDefinition : ScriptableObject
{
    public List<SliceTile> Tiles;
    public float Angle;
}
