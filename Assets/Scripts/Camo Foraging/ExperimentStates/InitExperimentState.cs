using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging {
    public class InitExperimentState : ExperimentController.ExperimentState
    {
        public override void EnterState()
        {
            // ec.SetState(new IdleExperimentState());
            Debug.Log("Found " + Display.displays.Length + " displays.");
            foreach (var d in Display.displays) {
                Debug.Log(d.ToString() + " is active? " + d.active);
            }

            if (Display.displays.Length >= 2) {
                Display.displays[1].Activate();
            }

            ViveProEyeTracker.GotEyeDataImmediateCallback += RecordRawEyeData;
            GazeRaycaster.GotGazeRayCallback += RecordRaycastGazeData;
            
            SetState(new IdleExperimentState());
        }

        private void RecordRawEyeData(ViveSR.anipal.Eye.EyeData_v2 eyeData) {
            LSLEventRecorder.RecordViveProEyeData(eyeData, eyeData.verbose_data);
        }

        private void RecordRaycastGazeData(RaycastHit hit, bool valid, ViveSR.anipal.Eye.EyeData_v2 eyeData) {
            LSLEventRecorder.RecordGazeData(valid, valid ? hit.point : Vector3.zero, valid ? hit.collider.transform : null, eyeData,
                ExperimentController.GetAngles(Vector3.forward, (valid ? hit.point : Vector3.zero) - ExperimentController.HMD.position),
                ExperimentController.GetAngles(ExperimentController.HMD.forward, (valid ? hit.point : Vector3.zero) - ExperimentController.HMD.position)
            );
            LSLEventRecorder.RecordGazeStringData(valid, valid ? hit.collider.transform : null, eyeData);
        }
    }

}
