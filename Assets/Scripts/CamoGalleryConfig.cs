using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CamoGalleryConfig", menuName = "CamoForaging/Camo Gallery Config", order = 3)]
public class CamoGalleryConfig : ScriptableObject
{
    // each gallery is for a different color channel changed, with the first in the row always being the target character
    public Material mainCamoPattern, distractorCamoPattern;
    public Material[] camoPatterns;
    public float[] distances;
    public AnimationClip[] poses;
    public int poseCount;
    public GameObject characterPrefab;
    public string[] camoTransformPaths;
}
