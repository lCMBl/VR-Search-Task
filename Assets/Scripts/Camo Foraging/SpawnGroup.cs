using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging {
    [System.Serializable]
    public class SpawnGroup
    {
        public int lowerDirectionBound, upperDirectionBound;
        public int halfNumberTargets, halfNumberDistractors;
        public Vector3[] spawnPositions;
    }

}
