using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CamoForaging {
    public class StopExperimentState : ExperimentController.ExperimentState
    {
        public override void EnterState()
        {
            ec.GetMenuItem<Button>("Stop Button").interactable = false;
            ec.GetMenuItem<Button>("Run Exp Button").interactable = true;
            ec.GetMenuItem<Button>("Run Tut Button").interactable = true;
            ec.GetMenuItem<Button>("Run Instructions Button").interactable = true;
            ec.GetMenuItem<Dropdown>("Experiment Select Dropdown").interactable = true;
            ec.laserSniperController.Deactivate();
            ec.spawnController.Deactivate();
            LSLEventRecorder.RecordSessionEvent("Stopped Experiment " + ExperimentController.SelectedBlockID.ToString(), "Experiment");

            SetState(new IdleExperimentState());
        }
    }

}
