using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CamoForaging {
    public class RunBlockExperimentState : ExperimentController.ExperimentState
    {
        public override void EnterState()
        {
            ec.spawnController.Activate();
            ec.laserSniperController.Activate();
            ec.GetMenuItem<Button>("Stop Button").interactable = true;
            ec.GetMenuItem<Button>("Run Exp Button").interactable = false;
            ec.GetMenuItem<Button>("Run Tut Button").interactable = false;
            ec.GetMenuItem<Button>("Run Instructions Button").interactable = false;
            ec.GetMenuItem<Dropdown>("Experiment Select Dropdown").interactable = false;
            LSLEventRecorder.RecordSessionEvent("Started Experiment " + ExperimentController.SelectedBlockID.ToString(), "Experiment");
            LSLEventRecorder.RecordSessionEvent("Program Version: " + Application.version, "Application");
        }

        public override void Tick()
        {
            if (!ec.RunMode) {
                SetState(new StopExperimentState());
            }
        }
    }

}
