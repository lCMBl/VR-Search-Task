using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace CamoForaging.Spawner {
    public class GenerateSpawnPositionsSpawnerState : SpawnController.SpawnerState
    {

        // private Trial trial;

        public override void EnterState() {
            // set the trial's spawn center direction
            ExperimentController.CurrentTrial.spawnCenterDirection = GetHeadDirection();
            // Debug.LogFormat("Spawn center direction is {0} with head direction of {1} and random of {2}", hd+rd, hd, rd);
            var uTri = GetUnwrappedTargetRangeIndexes(ExperimentController.CurrentTrial, sc.spawnRanges);
            // var tri = GetTargetRangeIndexes(ExperimentController.CurrentTrial, sc.spawnRanges);
            // Debug.LogFormat("Resulting target indexes are: [{0}]", string.Join(", ", tri));
            var uDri = GetUnwrappedDistractorRangeIndexes(ExperimentController.CurrentTrial, sc.spawnRanges, uTri);

            // var dri = GetDistractorRangeIndexes(ExperimentController.CurrentTrial, sc.spawnRanges, tri);
            ExperimentController.CurrentTrial.targetPositions = GenerateSpawnPositions(sc.spawnerOrigin, WrapRangeIndexes(uTri, sc.spawnRanges), sc.spawnRanges);
            ExperimentController.CurrentTrial.distractorPositions = GenerateSpawnPositions(sc.spawnerOrigin, WrapRangeIndexes(uDri, sc.spawnRanges), sc.spawnRanges);
            SetState(new SpawnGroupSpawnerState());
        }

        public float GetHeadDirection() {
            Vector3 headXZDir = ExperimentController.HMD.forward;
            headXZDir.y = 0f;
            // needs to be down so that the signed angles go in the right direction.
            return Vector3.SignedAngle(headXZDir, Vector3.forward, Vector3.down); 
        }

        public int GetRangeIndexFromDegrees(float degrees, SpawnRanges sr) {
            // gets the nearest (rounded down) index to use with the spawn ranges
            // if (degrees < 0) {
            //     Debug.Log(">>> " + degrees);
            // }
            return Mathf.FloorToInt( ( (360 + (degrees % 360)) % 360 ) / sr.DegreesPerRangeSegment);
        }

        public int GetRangeIndexFromDegreesNoWrap(float degrees, float degreesPerRangeSegment) {
            return Mathf.FloorToInt( degrees / degreesPerRangeSegment);
        }

        public float GetTargetRangeStartDegree(Trial t, SpawnRanges sr) {
            
            if (t.numTargets % 2 == 0) {
                // if trial is even, take the half distance from the center || subtract 1 to get number of steps to walk back.
                return (t.spawnCenterDirection - (t.interTargetDistance / 2f)) - (Mathf.Floor((t.numTargets/2f)-1) * t.interTargetDistance);
            } else {
                // if the number of trials is odd, do not subtract 1
                return t.spawnCenterDirection - (Mathf.Floor(t.numTargets/2f) * t.interTargetDistance);
            }
        }

        // TODO: make this return the unwrapped indexes instead of the wrapped ones, then wrap later
        public int[] GetTargetRangeIndexes(Trial t, SpawnRanges sr) {
            float startDegree = GetTargetRangeStartDegree(t, sr);
            List<int> result = new List<int>();
            for(int i = 0; i < t.numTargets; i++) {
                result.Add(GetRangeIndexFromDegrees(startDegree + (i * t.interTargetDistance), sr));
            }
            return result.ToArray();
        }

        public int[] GetUnwrappedTargetRangeIndexes(Trial t, SpawnRanges sr) {
            float startDegree = GetTargetRangeStartDegree(t, sr);
            int[] result = new int[t.numTargets];
            for (int i = 0; i < t.numTargets; i++) {
                result[i] = GetRangeIndexFromDegreesNoWrap(startDegree + (i * t.interTargetDistance), sr.DegreesPerRangeSegment);
            }
            return result;
        }

        public int[] WrapRangeIndexes(int[] unwrappedIndexes, SpawnRanges sr) {
            int[] result = new int[unwrappedIndexes.Length];
            for (int i = 0; i < unwrappedIndexes.Length; i++) {
                result[i] = (sr.NumRangeSegments + (unwrappedIndexes[i] % sr.NumRangeSegments)) % sr.NumRangeSegments;
            }
            return result;
        }

        public int[] GetUnwrappedDistractorRangeIndexes(Trial t, SpawnRanges sr, int[] unwrappedTargetIndexes) {
            // get the starting and ending range index
            int startIndex = unwrappedTargetIndexes.First() - GetRangeIndexFromDegreesNoWrap(t.interTargetDistance, sr.DegreesPerRangeSegment);
            int endIndex = unwrappedTargetIndexes.Last() + GetRangeIndexFromDegreesNoWrap(t.interTargetDistance, sr.DegreesPerRangeSegment);

            // create available Values set
            HashSet<int> availableValues = new HashSet<int>();
            for (int i = startIndex; i < endIndex; i++) {availableValues.Add(i);}

            // remove target range indexes and buffer zone from available values
            foreach(var ti in unwrappedTargetIndexes) {
                availableValues.Remove(ti);
                for(int si = 1; si <= ExperimentController.MinNumRangeSeparation; si++) {
                    availableValues.Remove(ti + si);
                    availableValues.Remove(ti - si);
                }
            }

            // choose distractor indexes from remaining values, record choices,
            // and remove distractor and buffers from availableValues
            int[] results = new int[t.numDistractors];
            for(int i = 0; i < t.numDistractors; i++) {
                results[i] = availableValues.ElementAt(Random.Range(0, availableValues.Count));
                availableValues.Remove(results[i]);
                for(int si = 1; si <= ExperimentController.MinNumRangeSeparation; si++) {
                    availableValues.Remove(results[i] + si);
                    availableValues.Remove(results[i] - si);
                }
            }

            return results;
        }

        public int[] GetDistractorRangeIndexes(Trial t, SpawnRanges sr, int[] targetRangeIndexes) {
            // meant to be called after target range indexes are known. 
            // spawn configuration must meet following conditions: 
            // no distractor with the same index as a target or other distractor
            // max +/- 1 inter target distance from the first and last targets
            // equal split of distractors on each side of the center angle (as best as possible. should be even number)
            float startDegree = GetTargetRangeStartDegree(t, sr);
            float minDegree = startDegree - t.interTargetDistance;
            float maxDegree = t.spawnCenterDirection + (t.spawnCenterDirection - minDegree);
            
            HashSet<int> availableValues = new HashSet<int>();
            for (int i = 0; i < sr.NumRangeSegments; i++) {availableValues.Add(i);}
            // remove target range indexes and buffer zone from available values
            foreach(var ti in targetRangeIndexes) {
                availableValues.Remove(ti);
                for(int si = 1; si <= ExperimentController.MinNumRangeSeparation; si++) {
                    availableValues.Remove((ti + si) % sr.NumRangeSegments);
                    availableValues.Remove((sr.NumRangeSegments + (ti - si)) % sr.NumRangeSegments);
                }
            }
            // remove all available values that fall outside of the spawn range
            availableValues.RemoveWhere(value => {
                //TODO use unwrapped range indexes, and only add available values up to the inter target distance
                // on each side, instead of creating a bunch and then removeWhere-ing them.
                return value < Mathf.Max(targetRangeIndexes.Min() - ExperimentController.MinNumRangeSeparation, 0);
            });

            //add the target indexes and a buffer to the taken values
            // HashSet<int> takenValues = new HashSet<int>(targetRangeIndexes);
            // foreach(var ti in targetRangeIndexes) {
            //     for(int si = 1; si <= ExperimentController.MinNumRangeSeparation; si++) {
            //         takenValues.Add((ti + si) % sr.NumRangeSegments);
            //         takenValues.Add((sr.NumRangeSegments + (ti - si)) % sr.NumRangeSegments);
            //     }
            // }

            List<int> result = new List<int>();
            // add distractor indexes, respecting the min range separation
            for(int i = 0; i < t.numDistractors; i++) {
                int chosenIndex = availableValues.ElementAt(Random.Range(0, availableValues.Count));
                // once a spawn range has been chosen, remove it and buffer ranges from available values
                result.Add(chosenIndex);
                availableValues.Remove(chosenIndex);
                for(int si = 1; si <= ExperimentController.MinNumRangeSeparation; si++) {
                    availableValues.Remove((chosenIndex + si) % sr.NumRangeSegments);
                    availableValues.Remove((sr.NumRangeSegments + (chosenIndex - si)) % sr.NumRangeSegments);
                }
            }
            return result.ToArray();
            
            
            // for (int i = 0; i < t.numDistractors; i++) {
            //     float upperRange = i % 2 == 0 ? t.spawnCenterDirection : maxDegree;
            //     float lowerRange = i % 2 == 0 ? minDegree : t.spawnCenterDirection;
            //     // Debug.LogWarningFormat("Found range as: [{0}, {1}) with i of  {2}", lowerRange, upperRange, i);
            //     bool foundValue = false;
            //     int errorCountThreshold = 1000, errorCount = 0;
    
            //     do {
            //         if (errorCount >= errorCountThreshold) {
            //             Debug.LogError("Unable to find a range index for distractor. Are there enough directions to choose from?");
            //             break;
            //         }
            //         int distractorRangeIndex = GetRangeIndexFromDegrees(Random.Range(0f, 359f), sr);//(sr.maxRanges.Length + Random.Range(lowerRange, upperRange)) % sr.maxRanges.Length;
            //         if (!takenValues.Contains(distractorRangeIndex)) {
            //             result.Add(distractorRangeIndex);
            //             takenValues.Add(distractorRangeIndex);
            //             foundValue = true;
            //         }
            //         errorCount++;
            //     } while(!foundValue);
                
            // }
            // return result.ToArray();
        }

        public Vector3[] GenerateSpawnPositions(Transform origin, int[] spawnRangeIndexes, SpawnRanges sr) {
            // for each range, move along the given direction to a random value inside of the spawn range
            // then, raycast down to the terrain, recording the surface value. This is the spawn position
            List<Vector3> positions = new List<Vector3>();
            foreach(var sri in spawnRangeIndexes) {
                origin.rotation = Quaternion.identity;
                origin.rotation = Quaternion.Euler(0f, sr.DegreesPerRangeSegment * sri, 0f);
                // origin.Rotate(Vector3.up * sr.DegreesPerRangeSegment * sri);
                // Debug.Log(">>> " + sri);
                // Debug.Log(sr.maxRanges.Length);
                float distance = Random.Range(sr.minRange, sr.maxRanges[sri]);
                Vector3 rayOrigin = (origin.forward * sr.minRange) + (origin.forward * distance) + origin.position;
                // make the raycast
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f, ~LayerMask.NameToLayer("Terrain"))) {
                    positions.Add(hit.point);
                } else {
                    Debug.LogError("Spawn Position Placement Ray didn't hit anything. Where is the terrain?");
                }
                
            }
            return positions.ToArray();
        }
    }
}
