using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging {

    public partial class ExperimentState : FSMState
    {
        protected ExperimentController ec;
        public override void InitState(FSMController c)
        {
            base.InitState(c);
            this.ec = c as ExperimentController;
        }
    }

}
