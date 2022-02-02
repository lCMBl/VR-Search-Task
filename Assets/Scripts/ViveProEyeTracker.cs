using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Runtime.InteropServices;
using Unity.XR.OpenVR;
using AOT;

// contains the link to the Vive Pro Eye, pushes out LSL events through LSL event recorder, move LSL outlet description to the same place as the other outlets.
public class ViveProEyeTracker : MonoBehaviour
{

    public delegate void GotEyeData(EyeData_v2 eyeData);
    public static GotEyeData GotEyeDataCallback;

    public static GotEyeData GotEyeDataImmediateCallback;
    public static EyeData_v2 CurrentEyeData {get; private set;}

    private bool eye_callback_registered = false;

    private static Queue<EyeData_v2> eyeDataQueue = new Queue<EyeData_v2>();

    // void Awake() {
    //     // for whatever reason, SRanipal ignores the values set in the inspector.
    //     // is this too early?
    //     SRanipal_Eye_Framework.Instance.EnableEyeVersion = SRanipal_Eye_Framework.SupportedEyeVersion.version2;
    //     SRanipal_Eye_Framework.Instance.EnableEyeDataCallback = true;
            // this ^ STILL doesn't work. Had to go to the eye framework directly to assign
            // version 2 as default. WHY??? BOTH OF THESE VALUES MUST BE SET MANUALLY
    // }

    // Start is called before the first frame update
    void Start()
    {
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }
        // SRanipal_Eye_Framework.Instance.EnableEyeVersion = SRanipal_Eye_Framework.SupportedEyeVersion.version2;
        // SRanipal_Eye_Framework.Instance.EnableEyeDataCallback = true;
        // GotEyeDataCallback += TestCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
        {
            SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
        }
        else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }

        while (eyeDataQueue.Count > 0) {
            var ed = eyeDataQueue.Dequeue();
            // var watch = System.Diagnostics.Stopwatch.StartNew();
            if (GotEyeDataCallback != null) {
                GotEyeDataCallback.Invoke(ed);
            }
            // Debug.Log("dequed eye data: " + ed);
            // watch.Stop();
            // Debug.LogWarning("Eye callback took " + watch.ElapsedMilliseconds + "ms");
        }


    }

    public bool RunEyeCalibration() {
        return SRanipal_Eye_v2.LaunchEyeCalibration();
    }

    void OnDisable() {
        Debug.Log("Called Disable");
        // var ovl = new OpenVRLoader();
        // ovl.Stop();
        // Release();
        // clears delegate(?)
        // GotEyeDataCallback = null;
        // GotEyeDataImmediateCallback = null;

    }

    private void Release() {
        Debug.Log("Called release");
        if (eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
        // clears delegate(?)
        // GotEyeDataCallback = null;
    }

    [MonoPInvokeCallback(typeof(GotEyeData))]
    private static void EyeCallback(ref EyeData_v2 eyeData) {
        CurrentEyeData = eyeData;
        if (GotEyeDataImmediateCallback != null) {
            GotEyeDataImmediateCallback.Invoke(eyeData);
        }
        eyeDataQueue.Enqueue(eyeData);
    }

    // private void TestCallback(EyeData_v2 eyeData) {
    //     Debug.Log("Got eye data: " + eyeData.frame_sequence + "," + eyeData.timestamp);
    // }
}
