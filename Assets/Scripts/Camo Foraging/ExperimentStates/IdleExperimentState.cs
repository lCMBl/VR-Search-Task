using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging {
    public class IdleExperimentState : ExperimentController.ExperimentState
    {
        public override void Tick()
        {
            if (ec.RunMode) {
                SetState(new RunBlockExperimentState());
            }
        }
    }

}
