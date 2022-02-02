using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CamoForaging.Spawner {
    public class LoadGroupSpawnerState : SpawnerState
    {
        // need to have number of targets, number of distractors, and spawn wedge
        private int[] lowerDirections, upperDirections;
        private List<Vector3> spawnPositions = new List<Vector3>();

        // public override void EnterState() {
        //     // pick which directions entities should spawn in. need to balance target positions left and right.
        //     int min = sc.currentSpawnGroup.lowerDirectionBound;
        //     int max = sc.currentSpawnGroup.upperDirectionBound;
        //     int totalNum = sc.currentSpawnGroup.halfNumberDistractors + sc.currentSpawnGroup.halfNumberTargets;
        //     int halfDirectionDelta = (int)(max - min)/2;
            
        //     lowerDirections = GetDirections(min, min + halfDirectionDelta, totalNum);
        //     upperDirections = GetDirections(min + halfDirectionDelta, max, totalNum);
        //     Debug.LogWarning(string.Join(", ", lowerDirections));
        //     Debug.LogWarning(string.Join(", ", upperDirections));

        //     // go through directions, and use the range for each direction to select a distance from the center.
        //     // then, raycast down to find the terrain surface (might be a special physics call for it?)
        //     // once complete, go to the group spawner and create the group.
        //     // might have to timeslice raycasts?
        // }

        public override void Tick()
        {
            int rayCount = 0;
            // timeslice raycasts (only a handful per frame)
            while (rayCount < sc.numberPositionChecksPerFrame && spawnPositions.Count < lowerDirections.Length + upperDirections.Length) {
                
                rayCount++;
            }

        }

        // gets a set of directions without replacement
        // add 1 to min so that halves can't overlap
        // quick and dirty. Assuming that there are many more potential directions than number of
        // directions to be selected, this is better than starting with a full list, and removing
        //  random elements until you have what you need.
        int[] GetDirections(int min, int max, int numDirections) {
            if (numDirections <= Mathf.Abs(max - (min + 1))) {
                HashSet<int> directions = new HashSet<int>();
                for (int i = 0; i < numDirections; i++) {
                    int numFound = directions.Count;
                    do {
                        int direction = Random.Range(min+1, max);
                        if (!directions.Contains(direction)) {
                            directions.Add(direction);
                        }
                    } while(directions.Count <= numFound);
                }
                int[] newDirections = new int[directions.Count];
                directions.CopyTo(newDirections);
                return newDirections;
            } else {
                Debug.LogError("Not enough different directions in range between min and max");
                return new int[0];
            }
        }
    }
}
