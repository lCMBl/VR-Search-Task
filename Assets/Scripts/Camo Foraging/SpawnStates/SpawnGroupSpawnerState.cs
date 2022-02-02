using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CamoForaging.Spawner {
    public class SpawnGroupSpawnerState : SpawnController.SpawnerState
    {

        private float timer = 0f;
        private bool fadingIn = true;
        private GameObject go;
        private List<Vector3> allPositions = new List<Vector3>();
        private Material m;
        public override void EnterState() {
            m = sc.screenFader.GetComponent<MeshRenderer>().material;
            sc.screenFader.SetActive(true);            
        }

        public override void Tick()
        {
            timer += Time.deltaTime;
            float fadeAmt = fadingIn ? timer : sc.screenFadeTime - timer;

            m.color = new Color(m.color.r, m.color.g, m.color.b, Mathf.Lerp(0f, 1f, fadeAmt / sc.screenFadeTime)); 
            if (fadingIn && timer >= sc.screenFadeTime) {
                m.color = Color.black;
                // sc.DestroySpawnedObjects();
                sc.DespawnAll();
                SpawnTrial();
                sc.ambientBattleSound.Play();
                fadingIn = false;
                timer = 0;
                LSLEventRecorder.RecordSessionEvent("Trial Start", "Experiment");
            }

            if (!fadingIn && timer >= sc.screenFadeTime) {
                m.color = Color.clear;
                sc.screenFader.SetActive(false);
                SetState(new WaitForNextTrialSpawnerState());
            }


        }

        private void SpawnTrial() {
            allPositions.AddRange(ExperimentController.CurrentTrial.targetPositions);
            allPositions.AddRange(ExperimentController.CurrentTrial.distractorPositions);


            // foreach (var tPos in ExperimentController.CurrentTrial.targetPositions) {
            //     // GameObject.Instantiate(sc.debugMarker, tPos, Quaternion.identity);
            //     go = GameObject.Instantiate(sc.targetObject, tPos, Quaternion.identity);//, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            //     go.transform.rotation = RotateTowards(go.transform.position, allPositions[Random.Range(0, allPositions.Count)]);
            //     go.transform.SetParent(sc.transform);
            // }

            // var distractorObject = ExperimentController.CurrentTrial.useHardDistractors ? sc.hardDistractorObject : sc.easyDistractorObject;
            var distractorObjectPool = ExperimentController.CurrentTrial.useHardDistractors ? sc.hardDistractorPool : sc.easyDistractorPool;

            // foreach (var dPos in ExperimentController.CurrentTrial.distractorPositions) {
            //     // GameObject.Instantiate(sc.debugMarker, dPos, Quaternion.identity);
            //     go = GameObject.Instantiate(distractorObject, dPos, Quaternion.identity);// Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            //     go.transform.rotation = RotateTowards(go.transform.position, allPositions[Random.Range(0, allPositions.Count)]);
            //     go.transform.SetParent(sc.transform);
            // }

            sc.Spawn(sc.targetPool, ExperimentController.CurrentTrial.targetPositions, allPositions.ToArray());
            sc.Spawn(distractorObjectPool, ExperimentController.CurrentTrial.distractorPositions, allPositions.ToArray());
            
            // record trial info
            LSLEventRecorder.RecordTrialParameters(ExperimentController.CurrentTrial);
        }

        private Quaternion RotateTowards(Vector3 origin, Vector3 position) {
            var dir = position - origin;
            dir.y = 0f;
            return Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}
