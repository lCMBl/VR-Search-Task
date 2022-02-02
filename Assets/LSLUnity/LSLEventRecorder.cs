using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamoForaging.Spawner;
using CamoForaging.LaserIndicator;
using CamoForaging.ExperimentControl;
using ViveSR.anipal.Eye;

// IMPORTANT: THIS FILE IS COMPUTER GENERATED
// Do not make edits directly to LSLEventRecorder.cs
// Instead, define LSL streams using LSLStreamDescription
// objects, add them to the LSLStreamManager object, and
// click 'Generate LSLEventRecorder.cs'

public class LSLEventRecorder
{
    private static LSLStreamManager lslSM;

    public static void Init(LSLStreamManager streamManager) {
        lslSM = streamManager;
    }

    public static void RecordParticipantActions(string actionName, string actionSubjectName) {
    lslSM.GetOutlet("ParticipantActions").push_sample(new string[] {
        actionName, //ActionName
        actionSubjectName, //ActionSubjectName
        LSLStreamManager.FrameID.ToString() //UnityFrameID
    });
}

    public static void RecordSessionEvent(string value, string topic) {
    lslSM.GetOutlet("SessionEvent").push_sample(new string[] {
        value, //EventText
        topic, //EventTopic
        LSLStreamManager.FrameID.ToString() //UnityFrameID
    });
}

    public static void RecordHeadPose(Transform participantHMD) {
    lslSM.GetOutlet("HeadPose").push_sample(new float[] {
        participantHMD.position.x, //PositionX
        participantHMD.position.y, //PositionY
        participantHMD.position.z, //PositionZ
        participantHMD.eulerAngles.x, //RotationX
        participantHMD.eulerAngles.y, //RotationY
        participantHMD.eulerAngles.z, //RotationZ
        participantHMD.forward.x, //ForwardX
        participantHMD.forward.y, //ForwardY
        participantHMD.forward.z, //ForwardZ
        ((float)LSLStreamManager.FrameID) //UnityFrameID
    });
}

    public static void RecordViveController(Transform controller) {
    lslSM.GetOutlet("ViveController").push_sample(new float[] {
        int.Parse(controller.name.Split(':')[1]), //ControllerID
        controller.position.x, //PositionX
        controller.position.y, //PositionY
        controller.position.z, //PositionZ
        controller.eulerAngles.x, //RotationX
        controller.eulerAngles.y, //RotationY
        controller.eulerAngles.z, //RotationZ
        controller.forward.x, //ForwardX
        controller.forward.y, //ForwardY
        controller.forward.z, //ForwardZ
        ((float)LSLStreamManager.FrameID) //UnityFrameID
    });
}

    public static void RecordTrialParameters(Trial trial) {
    lslSM.GetOutlet("TrialParameters").push_sample(new float[] {
        trial.useHardDistractors ? 1f : 0f, //DistractorType
        trial.numTargets, //NumberOfTargets
        trial.numDistractors, //NumberOfDistractors
        trial.spawnCenterDirection, //SpawnCenterDirection
        trial.interTargetDistance, //InterTargetDistance
        trial.delayTargetPresentation, //PresentationOnsetDelay
        ((float)LSLStreamManager.FrameID) //UnityFrameID
    });
}

    public static void RecordCharacterNumericalData(Transform target, Transform participantHMD) {
    lslSM.GetOutlet("CharacterNumericalData").push_sample(new float[] {
        float.Parse(target.name.Split('[', ']')[1]), //CharacterID
        target.position.x, //PositionX
        target.position.y, //PositionY
        target.position.z, //PositionZ
        target.rotation.eulerAngles.x, //RotationX
        target.rotation.eulerAngles.y, //RotationY
        target.rotation.eulerAngles.z, //RotationZ
        target.forward.x, //ForwardX
        target.forward.y, //ForwardY
        target.forward.z, //ForwardZ
        Vector3.Distance(participantHMD.position, target.position), //DistanceToParticipant
        ((float)LSLStreamManager.FrameID) //UnityFrameID
    });
}

    public static void RecordCharacterStringData(Transform target) {
    lslSM.GetOutlet("CharacterStringData").push_sample(new string[] {
        target.name.Split('[', ']')[1], //CharacterID
        target.name, //Name
        LSLStreamManager.FrameID.ToString() //UnityFrameID
    });
}

    public static void RecordCharacterAnimationEvent(int characterID, string eventName) {
    lslSM.GetOutlet("CharacterAnimationEvent").push_sample(new string[] {
        characterID.ToString(), //CharacterID
        eventName, //EventName
        LSLStreamManager.FrameID.ToString() //UnityFrameID
    });
}

    public static void RecordGazeData(bool valid, Vector3 gazePoint, Transform target, EyeData_v2 eyeData, Vector3 worldAngles, Vector3 eyeInHeadAngles) {
    lslSM.GetOutlet("GazeData").push_sample(new float[] {
        eyeData.timestamp, //EyeDataTimestamp
        eyeData.frame_sequence, //EyeDataFrameSequence
        valid ? 1f : 0f, //GazeValidity
        gazePoint.x, //GazePointPositionX
        gazePoint.y, //GazePointPositionY
        gazePoint.z, //GazePointPositionZ
        target != null ? target.transform.InverseTransformPoint(gazePoint).x : 0f, //GazePointRelativePositionX
        target != null ? target.transform.InverseTransformPoint(gazePoint).y : 0f, //GazePointRelativePositionY
        target != null ? target.transform.InverseTransformPoint(gazePoint).z : 0f, //GazePointRelativePositionZ
        worldAngles.z, //GazeAngle
        worldAngles.x, //GazeAngleX
        worldAngles.y, //GazeAngleY
        eyeInHeadAngles.z, //EyeinHeadAngle
        eyeInHeadAngles.x, //EyeinHeadAngleX
        eyeInHeadAngles.y, //EyeinHeadAngleY
        ((float)LSLStreamManager.FrameID) //UnityFrameID
    });
}

    public static void RecordGazeStringData(bool valid, Transform target, EyeData_v2 eyeData) {
    lslSM.GetOutlet("GazeStringData").push_sample(new string[] {
        eyeData.timestamp.ToString(), //EyeDataTimestamp
        eyeData.frame_sequence.ToString(), //EyeDataFrameSequence
        valid ? "1" : "0", //GazeValidity
        target != null ? target.name : "None", //GazeTargetName
        LSLStreamManager.FrameID.ToString() //UnityFrameID
    });
}

    public static void RecordViveProEyeData(EyeData_v2 eyeData, VerboseData vb) {
    lslSM.GetOutlet("ViveProEyeData").push_sample(new float[] {
        eyeData.timestamp, //EyeDataTimestamp
        eyeData.frame_sequence, //EyeDataFrameSequence
        vb.combined.convergence_distance_mm, //CombinedConvergenceDistance
        vb.combined.convergence_distance_validity ? 1f: 0f, //CombinedConvergenceDistanceValidity
        vb.combined.eye_data.eye_data_validata_bit_mask, //CombinedEyeDataValidityBitmask
        vb.combined.eye_data.eye_openness, //CombinedEyeOpenness
        vb.combined.eye_data.gaze_direction_normalized.x, //CombinedGazeDirectionX
        vb.combined.eye_data.gaze_direction_normalized.y, //CombinedGazeDirectionY
        vb.combined.eye_data.gaze_direction_normalized.z, //CombinedGazeDirectionZ
        vb.combined.eye_data.gaze_origin_mm.x, //CombinedGazeOriginX
        vb.combined.eye_data.gaze_origin_mm.y, //CombinedGazeOriginY
        vb.combined.eye_data.gaze_origin_mm.z, //CombinedGazeOriginZ
        vb.combined.eye_data.pupil_diameter_mm, //CombinedPupilDiameter
        vb.combined.eye_data.pupil_position_in_sensor_area.x, //CombinedPupilPositionX
        vb.combined.eye_data.pupil_position_in_sensor_area.y, //CombinedPupilPositionY
        vb.left.eye_data_validata_bit_mask, //LeftEyeDataValidityBitmask
        vb.left.eye_openness, //LeftEyeOpenness
        vb.left.gaze_direction_normalized.x, //LeftGazeDirectionX
        vb.left.gaze_direction_normalized.y, //LeftGazeDirectionY
        vb.left.gaze_direction_normalized.z, //LeftGazeDirectionZ
        vb.left.gaze_origin_mm.x, //LeftGazeOriginX
        vb.left.gaze_origin_mm.y, //LeftGazeOriginY
        vb.left.gaze_origin_mm.z, //LeftGazeOriginZ
        vb.left.pupil_diameter_mm, //LeftPupilDiameter
        vb.left.pupil_position_in_sensor_area.x, //LeftPupilPositionX
        vb.left.pupil_position_in_sensor_area.y, //LeftPupilPositionY
        vb.right.eye_data_validata_bit_mask, //RightEyeDataValidityBitmask
        vb.right.eye_openness, //RightEyeOpenness
        vb.right.gaze_direction_normalized.x, //RightGazeDirectionX
        vb.right.gaze_direction_normalized.y, //RightGazeDirectionY
        vb.right.gaze_direction_normalized.z, //RightGazeDirectionZ
        vb.right.gaze_origin_mm.x, //RightGazeOriginX
        vb.right.gaze_origin_mm.y, //RightGazeOriginY
        vb.right.gaze_origin_mm.z, //RightGazeOriginZ
        vb.right.pupil_diameter_mm, //RightPupilDiameter
        vb.right.pupil_position_in_sensor_area.x, //RightPupilPositionX
        vb.right.pupil_position_in_sensor_area.y, //RightPupilPositionY
        ((float)LSLStreamManager.FrameID) //UnityFrameID
    });
}

}
