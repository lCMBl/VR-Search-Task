using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging.LaserIndicator {
    public class LaserState : FSMState
    {
        protected LaserSniperController lc;
        public override void InitState(FSMController c)
        {
            base.InitState(c);
            this.lc = c as LaserSniperController;
        }
    }

}
