using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging.LaserIndicator {

    public class LaserSniperController : FSMController
    {
        // move all laser code and related interactions to this script, 
        // and related from participant Input.
        public LineRenderer laserBeam;
        public Transform handTransform;
        public float maxRayLength, negativeRadioMessageOffset;
        public AudioSource sniperRifleShot;
        public AudioSource negativeRadioMessage;
        private bool active;
        public bool Active {
            get { return active; }
        }
        // public GameObject hitObject;
        public class LaserState : FSMState
        {
            protected LaserSniperController lc;
            public override void InitState(FSMController c)
            {
                base.InitState(c);
                this.lc = c as LaserSniperController;
            }
        }
        public override FSMState GetInitialState() {
            return new InactiveLaserState();
        }

        public void Activate() {
            active = true;
            // SetState(new ActiveLaserState());
        }

        public void Deactivate() {
            active = false;
        }

    }

}
