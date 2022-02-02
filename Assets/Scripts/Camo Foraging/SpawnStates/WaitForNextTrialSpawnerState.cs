using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CamoForaging.Spawner {
    public class WaitForNextTrialSpawnerState : SpawnController.SpawnerState
    {

        
        public override void EnterState() {
            ParticipantInput.OnTouchpadPress += OnTouchpadPress;
        }

        public override void Tick()
        {
            if (!sc.Active) {
                SetState(new EndSpawnerState());
            }
        }

        public override void ExitState()
        {
            LSLEventRecorder.RecordSessionEvent("Trial End", "Experiment");
            ParticipantInput.OnTouchpadPress -= OnTouchpadPress;
        }

        public void OnTouchpadPress(bool leftTouchpadPressed) {
            LSLEventRecorder.RecordParticipantActions("Next Trial", "None");
            if (ExperimentController.NextTrial()) {
                SetState(new GenerateSpawnPositionsSpawnerState());
            } else {
                SetState(new EndSpawnerState());
            }
        }
    }
}
