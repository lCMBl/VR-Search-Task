using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging {
    public class SpawnPositions : ScriptableObject
    {
        // contains a list of all available spawn positions. Generated offline
        public float minRange;
        public float[] maxRanges;

        public void GeneratePositions(Transform origin, int numberOfPositions, float maxRange) {
            List<float> newMaxRanges = new List<float>();
            origin.Rotate(Vector3.zero, Space.World);
            RaycastHit hit;
            for (int i = 0; i < numberOfPositions; i++) {
                if (i > 0) {
                    origin.Rotate(Vector3.up * (360f/i), Space.World);
                }
                if (Physics.Raycast(origin.position, origin.forward, out hit, maxRange)) {
                    newMaxRanges.Add(Vector3.Distance(origin.position, hit.point) - 0.5f);
                } else {
                    // set to maximum range
                    newMaxRanges.Add(maxRange);
                }
            }
            maxRanges = newMaxRanges.ToArray();
        }
    }

}
