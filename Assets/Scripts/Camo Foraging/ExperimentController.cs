using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamoForaging.ExperimentControl;
using UnityEngine.UI;

namespace CamoForaging {
    public class ExperimentController : FSMController
    {

        private static ExperimentController instance;

        public class ExperimentState : FSMState {
            protected ExperimentController ec;
            public override void InitState(FSMController c)
            {
                base.InitState(c);
                this.ec = c as ExperimentController;
            }
        }
        

        [SerializeField]
        private Transform hmd, leftController, rightController;
        public static Transform HMD{
            get { return instance.hmd; }
        }
        public BlockDescription[] blockDescriptions;

        public BlockID selectedBlockID;
        public static BlockID SelectedBlockID {
            get { return instance.selectedBlockID; }
        }
        private static Trial[] trials;
        public static Trial[] Trials {
            get { return trials; }
            set { trials = value; }
        }
        private Trial currentTrial;

        private int currentTrialIndex = 0;

        public static Trial CurrentTrial {
            get { return instance.currentTrial; }
        }

        public Spawner.SpawnController spawnController;
        public LaserIndicator.LaserSniperController laserSniperController;

        private bool runMode;
        public bool RunMode {
            get { return runMode; }
        }

        public Canvas experimentUI;
        [SerializeField]
        private int minNumRangeSeparation; // how many ranges minimum between each character
        public static int MinNumRangeSeparation {
            get { return instance.minNumRangeSeparation; }
        }

        private Dictionary<string, Selectable> menuSelectables = new Dictionary<string, Selectable>();

        void Awake() {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            LoadMenuUI<Selectable>(menuSelectables);
            SetState(new InitExperimentState());
        }

        protected override void MonoBehaviourUpdate()
        {
            LSLEventRecorder.RecordHeadPose(hmd);
            LSLEventRecorder.RecordViveController(leftController);
            LSLEventRecorder.RecordViveController(rightController);
        }

        public override FSMState GetInitialState()
        {
            return new IdleExperimentState();
        }

        private void LoadMenuUI<T>(Dictionary<string, T> destinationDict) where T : Selectable  {
            // add the UI elements to the dictionaries by type and name
            destinationDict.Clear();
            var element = experimentUI.GetComponentsInChildren<T>();
            foreach (var e in element) {
                destinationDict.Add(e.name, e);
            }
        }

        public T GetMenuItem<T> (string name) where T : Selectable {
            if (menuSelectables.ContainsKey(name)) {
                return (T)menuSelectables[name];
            } else {
                return null;
            }
        } 



        public static void GenerateTrialsFromBlock() {
            Trials = new Trial[0];
            instance.blockDescriptions[(int)instance.selectedBlockID].GenerateTrials(instance);
        }

        public static void SetFirstTrial() {
            instance.currentTrialIndex = 0;
            instance.currentTrial = Trials[0];
        }

        public static bool NextTrial() {
            if (instance.currentTrialIndex < Trials.Length) {
                instance.currentTrialIndex++;
                instance.currentTrial = Trials[instance.currentTrialIndex];
                return true;
            } else {
                return false;
            }
        }

        // Methods called from UI events:
        public void SetBlockType(int val) {
            selectedBlockID = (BlockID)val;
        }

        public void RunEyeCalibration() {
            LSLEventRecorder.RecordSessionEvent("Run Eye Tracking Calibration", "Application");
            var calibrationResult = FindObjectOfType<ViveProEyeTracker>().RunEyeCalibration();
            Debug.LogFormat("Result of eye calibration: {0}", calibrationResult ? "Success" : "Failure");
            LSLEventRecorder.RecordSessionEvent("Eye Calibration Result:" + (calibrationResult ? "Success": "Failure"), "Application");
        
        }

        public void RunBlock() {
            runMode = true;
        }

        public void StopBlock() {
            runMode = false;
        }

        public static Vector3 GetAngles(Vector3 from, Vector3 to) {
            Vector3 xzf = new Vector3(from.x, 0f, from.z);
            Vector3 yzf = new Vector3(0f, from.y, from.z);
            
            Vector3 xzt = new Vector3(to.x, 0f, to.z);
            Vector3 yzt = new Vector3(0f, to.y, to.z);
            
            return new Vector3(
                Vector3.SignedAngle(yzf, yzt, Vector3.right),
                Vector3.SignedAngle(xzf, xzt, Vector3.up),
                Vector3.Angle(from, to)
            );
        }

        public static float WrapAngle(float angle) {
            return (angle + 180) % 360;
        }

    }

    [System.Serializable]
    public enum BlockID {
        BlockTypeA,
        BlockTypeB
    }
}
