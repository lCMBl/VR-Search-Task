using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CamoForaging.Spawner {
    public class EndSpawnerState : SpawnController.SpawnerState
    {

        public override void EnterState() {
            // sc.DestroySpawnedObjects();
            sc.DespawnAll();
            sc.ambientBattleSound.Stop();
        }

        public override void Tick()
        {
            if (sc.Active) {
                SetState(new InitSpawnerState());
            }
        }
    }
}
