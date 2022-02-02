using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CamoForaging.Spawner {
    public class BakeSpawnRangesSpawnerState : SpawnController.SpawnerState
    {

        private float timer = 0f, minRange, maxRange;
        private bool runOnce = true;
        private int numRanges;
        // creates and saves a spawn ranges scriptable object, with ranges according to the tre positions
        public BakeSpawnRangesSpawnerState(int numRanges, float minRange, float maxRange) {
            this.numRanges = numRanges;
            this.minRange = minRange;
            this.maxRange = maxRange;
        }
        public override void EnterState() {
            
            // Debug.DrawRay(sc.spawnerOrigin.position, sc.spawnerOrigin.forward * 100f, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 100f);
        }

        public override void Tick()
        {
            timer += Time.deltaTime;
            if (runOnce && timer >= 5f) {
                runOnce = false;
                
                sc.spawnRanges.minRange = minRange;
                sc.spawnRanges.GeneratePositions(sc.spawnerOrigin, numRanges, maxRange);
            }
            // timer += Time.deltaTime;
            // if (runOnce && timer >= 5f) {
            //     runOnce = false;
            //     Collider[] colliders = new Collider[800];
            //     int result = Physics.OverlapSphereNonAlloc(sc.transform.position, sc.testRadius, colliders);
            //     Debug.Log("Result of sphere overlap: " + result);
            //     foreach (var c in colliders) {
            //         if (c != null) {
            //             Debug.Log(c.name);
            //             GameObject.Instantiate(sc.debugMarker, c.transform.position, Quaternion.identity);
            //         }
                    
            //     }
            // }
        }
    }
}
