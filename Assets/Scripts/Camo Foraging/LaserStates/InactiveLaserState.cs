using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging.LaserIndicator {
    public class InactiveLaserState : LaserSniperController.LaserState
    {
        public override void Tick()
        {
            if (lc.Active) {
                SetState(new ActiveLaserState());
            }
        }
    }

}
