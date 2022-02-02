using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CamoForaging.Spawner {
    public class InitSpawnerState : SpawnController.SpawnerState
    {

        
        public override void EnterState() {
            // set the unity seed 
            Random.InitState(sc.seed);
            // generate the trials at runtime with the help of a block description
            ExperimentController.GenerateTrialsFromBlock();
            // sc.SetState(new BakeSpawnRangesSpawnerState(360, 10f, 100f));
            Debug.Log("Started Generating Trials");
            Debug.Log("Child count: " + sc.transform.childCount);
            if (sc.transform.childCount == 0) {
                // we havent initialized the spawning pool yet, so do so.
                
                sc.targetPool = FillSpawningPool(sc.spawningPoolSize, sc.targetObject, 100);
                sc.easyDistractorPool = FillSpawningPool(sc.spawningPoolSize, sc.easyDistractorObject, 200);
                sc.hardDistractorPool = FillSpawningPool(sc.spawningPoolSize, sc.hardDistractorObject, 300);
            }
        }

        public override void Tick()
        {
            if (ExperimentController.Trials.Length > 0) {
                Debug.LogFormat("Finished Generating {0} Trials", ExperimentController.Trials.Length);
                SetState(new GenerateSpawnPositionsSpawnerState());
                
            }
        }

        private GameObject[] FillSpawningPool(int poolSize, GameObject poolObject, int idOffset = 0) {
            // create a pool of the given objects
            List<GameObject> pool = new List<GameObject>();
            for(int i = 0; i < poolSize; i++) {
                GameObject go = GameObject.Instantiate(poolObject, Vector3.zero, Quaternion.identity);
                go.name = poolObject.name + "[" + (i + idOffset).ToString() + "]";
                go.transform.SetParent(sc.transform);
                go.SetActive(false);
                pool.Add(go);
            }

            return pool.ToArray();
        }
    }
}
