using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging {
    [System.Serializable]
    [CreateAssetMenu(fileName = "SpawnRanges", menuName = "CamoForaging/Spawn Ranges", order = 1)]
    public class SpawnRanges : ScriptableObject
    {
        // contains a list of all available spawn ranges. Generated offline
        public float minRange;
        public float[] maxRanges;

        public float DegreesPerRangeSegment {
            get {
                return 360f / maxRanges.Length;
            }
        }

        public int NumRangeSegments {
            get {
                return maxRanges.Length;
            }
        }

        // divides 360 degrees by the number of ranges. raycasts each direction,
        // then records the range at which an object was hit
        public void GeneratePositions(Transform origin, int numberOfRanges, float maxRange) {
            List<float> newMaxRanges = new List<float>();
            origin.rotation = Quaternion.identity;
            float degStep = 360f / numberOfRanges;
            RaycastHit hit;
            float range = maxRange;
            for (int i = 0; i < numberOfRanges; i++) {
                    // origin.Rotate(Vector3.up * (360f/i), Space.World);
                origin.rotation = Quaternion.Euler(0f, degStep * i, 0f);
                // for some reason, vegetation studio doesn't remove colliders when the actual objects are hidden by a mask.
                // so, start the raycast at min range, and go to max range (this should avoid the colliders nearby,
                // but make sure that the trees are hidden. )
                // TODO will hidden trees still block raycasts??
                var originPos = origin.position + (origin.forward * minRange);
                if (Physics.Raycast(originPos , origin.forward, out hit, maxRange)) {
                    range = Vector3.Distance(originPos, hit.point) - 0.5f;
                } 
                newMaxRanges.Add(range);
                Debug.DrawRay(originPos, origin.forward * range, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 120f);
            }
            maxRanges = newMaxRanges.ToArray();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
            Debug.Log("Finished Generating Spawn Ranges");
        }
    }

}
