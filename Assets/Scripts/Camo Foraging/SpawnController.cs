using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging.Spawner {
    public class SpawnController : FSMController
    {
        public int seed = 1992;
        public GameObject targetObject, easyDistractorObject, hardDistractorObject;
        public int numberPositionChecksPerFrame = 5;
        public Transform spawnerOrigin;
        public SpawnRanges spawnRanges;

        public GameObject screenFader;

        public AudioSource ambientBattleSound;
        public float screenFadeTime = 1f;
        public int spawningPoolSize;
        public GameObject[] targetPool, easyDistractorPool, hardDistractorPool;

        public int GetCharacterID(Transform target) {
            return int.Parse(target.name.Split(new char[] {'[', ']'})[1]);
            // int poolOffset = 0;
            // foreach (var pool in new GameObject[][] {targetPool, easyDistractorPool, hardDistractorPool}) { // jagged array (arrays in arrays)
            //     poolOffset += 100;
            //     var i = System.Array.FindIndex(pool, element => element == target.gameObject);
            //     if (i > 0) {
            //         // then we've found it. return the ID.
            //         return i + poolOffset;
            //     }
            // }
            // // if we've reached here, we didn't find anything. return -1
            // return -1;
        }
        
        // public BlockDescription debugBlockDescription;

        // private static Trial[] trials;
        // public static Trial[] Trials {
        //     get { return trials; }
        //     set { trials = value; }
        // }
        // public Trial currentTrial;
        // public int currentTrialIndex = 0;
        public bool bakeRanges;
        private bool active;
        public bool Active {
            get { return active; }
        }

        public class SpawnerState : FSMState
        {
            protected SpawnController sc;
            public override void InitState(FSMController c)
            {
                base.InitState(c);
                this.sc = c as SpawnController;
            }
        }

        // public SpawnGroup currentSpawnGroup, previousSpawnGroup;
        void Start() {
            // SetState(new InitSpawnerState());
        }
        

        // this is just for testing. once the actual experiment is set up, remove this update method,
        // and set FSM Controller currentState back to private
        void Update() {
            if (bakeRanges) {
                bakeRanges = false;
                SetState(new BakeSpawnRangesSpawnerState(360, 10f, 100f));
            }

            if (currentState != null) {
                currentState.Tick();
            }
        }

        public override FSMState GetInitialState()
        {
            return new EndSpawnerState();
        }

        public void Activate() {
            active = true;
            // SetState(new InitSpawnerState());
        }

        public void Deactivate() {
            active = false;
        }

        public void Spawn(GameObject[] pool, Vector3[] positions, Vector3[] rotationTargets) {
            int numSpawned = 0;
            foreach (var po in pool) {
                if (numSpawned < positions.Length) {
                    // position and activate pool object
                    po.transform.position = positions[numSpawned];
                    po.transform.rotation = RotateTowards(po.transform.position, rotationTargets[Random.Range(0, rotationTargets.Length)]);
                    po.SetActive(true);
                    po.GetComponent<Collider>().enabled = true;
                    // make sure the visual component lines up with the object
                    po.GetComponentInChildren<Animator>().transform.localPosition = Vector3.zero;

                    numSpawned++;
                } else {
                    // deactivate pool object
                    po.SetActive(false);
                }
                // record character data
                LSLEventRecorder.RecordCharacterAnimationEvent(
                    GetCharacterID(po.transform),
                    po.activeInHierarchy ? "Active" : "Inactive"
                );
                LSLEventRecorder.RecordCharacterNumericalData(po.transform, ExperimentController.HMD);
                LSLEventRecorder.RecordCharacterStringData(po.transform);
            }
            // move the battle sound emitter to the right spot.
            ambientBattleSound.transform.position = positions[0];
        }

        public void DespawnAll() {
            var numObjects = transform.childCount;
            for(int i = 0; i< numObjects; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void DestroySpawnedObjects() {
            // destroy all objects used in this trial
            var numObjects = transform.childCount;
            for(int i = numObjects-1; i >= 0; i-- ) {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        private Quaternion RotateTowards(Vector3 origin, Vector3 position) {
            var dir = position - origin;
            dir.y = 0f;
            return Quaternion.LookRotation(dir, Vector3.up);
        }

        // public bool NextTrial() {
        //     if (currentTrialIndex < Trials.Length) {
        //         currentTrialIndex++;
        //         currentTrial = Trials[currentTrialIndex];
        //         return true;
        //     } else {
        //         return false;
        //     }
        // }

        // timer += Time.deltaTime;
        //     if (runOnce && timer >= 5f) {
        //         runOnce = false;
        //         Collider[] colliders = new Collider[400];
        //         int result = Physics.OverlapSphereNonAlloc(sc.transform.position, sc.testRadius, colliders);
        //         Debug.Log("Result of sphere overlap: " + result);
        //         foreach (var c in colliders) {
        //             if (c != null) {
        //                 Debug.Log(c.name);
        //                 GameObject.Instantiate(sc.debugMarker, c.transform.position, Quaternion.identity);
        //             }
                    
        //         }
        //     }
    }
}
