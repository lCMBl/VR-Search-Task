using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(LSLStreamManager))]
public class LSLStreamManager_Inspector : Editor
{
    public TextAsset eventRecorderTemplate, recordStreamPartial;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        var lslSM = (LSLStreamManager)target;
        EditorGUILayout.Separator();
        if (GUILayout.Button("Generate LSLEventRecorder.cs")) {
            // Debug.Log("It's alive! " + lslSM.name);
            GenerateLSLEventRecorder(lslSM);
            Debug.Log("LSLEventRecorder.cs successfully generated");
        }
    }

    private void GenerateLSLEventRecorder(LSLStreamManager lslSM) {
        eventRecorderTemplate = (TextAsset)serializedObject.FindProperty("eventRecorderTemplate").objectReferenceValue;
        recordStreamPartial = (TextAsset)serializedObject.FindProperty("recordStreamPartial").objectReferenceValue;
        // Debug.Log(eventRecorderTemplate.text);
        // Debug.Log(recordStreamPartial.text);

        List<string> recordingFunctions = new List<string>();
        foreach (var sd in lslSM.streamDefinitions) {
            string rfs = recordStreamPartial.text;
            rfs = rfs.Replace("__STREAM_NAME__", sd.name.Replace(" ", ""));
            rfs = rfs.Replace("__STREAM_FORMAT_TYPE__", sd.SampleType);
            // rfs = rfs.Replace("__STREAM_FORMAT_TYPE__", sd.SampleType);

            // fill in arguments
            List<string> ias = new List<string>();
            foreach(var ia in sd.inputArguments) {
                ias.Add(string.Format("{0} {1}", ia.type, ia.variableName));
            }
            rfs = rfs.Replace("__ARGUMENT_INFO__", string.Join(", ", ias));

            // fill in channel argument paths
            List<string> cps = new List<string>();
            foreach(var cd in sd.channelDescriptions) {
                cps.Add(string.Format("{0}, //{1}",
                    cd.argumentPath,
                    cd.name.Replace(" ", "")
                ));
            }
            if (sd.includeUnityFrameIDChannel) {
                // if we need to include the unity frame sequence, add it to the end of the stream
                cps.Add(string.Format("{0} //UnityFrameID",
                    sd.SampleType.Contains("string") ? "LSLStreamManager.FrameID.ToString()" : "((" + sd.SampleType + ")LSLStreamManager.FrameID)"
                ));
            }
            rfs = rfs.Replace("__CHANNEL_ARGUMENT_PATHS__", string.Join("\n        ", cps));
            // Debug.Log(rfs);
            recordingFunctions.Add(rfs);
        }


        // save our new LSL Event Recorder file
        using(StreamWriter sw = new StreamWriter(Application.dataPath + "/LSLUnity/LSLEventRecorder.cs")) {
            sw.Write(
                eventRecorderTemplate.text.Replace(
                    "__RECORDING_FUNCTIONS__",
                    string.Join("\n\n    ", recordingFunctions)
                )
            );
        }
        //Refresh the Asset Database
        AssetDatabase.Refresh();
    }
}
