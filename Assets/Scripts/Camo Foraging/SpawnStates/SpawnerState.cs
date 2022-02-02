using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging.Spawner {
    public class SpawnerState : FSMState
    {
        protected SpawnController sc;
        public override void InitState(FSMController c)
        {
            base.InitState(c);
            this.sc = c as SpawnController;
        }
    }

}
