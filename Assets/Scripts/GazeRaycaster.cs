using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;

public class GazeRaycaster : MonoBehaviour
{
    // provides raycast information based on gaze data.
    private static GazeRaycaster instance;

    public delegate void GotGazeRay(RaycastHit hit, bool valid, EyeData_v2 eyeData);

    public static GotGazeRay GotGazeRayCallback;
    public float maxRayLength = 100f;
    private Transform HMD;

    private RaycastHit hit;
    private bool validHit = false;
    private bool validData = false;

    public static RaycastHit Hit {get => instance.hit;}
    public static bool IsHitValid {get => instance.validData && instance.validHit;}



    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ViveProEyeTracker.GotEyeDataCallback += RaycastGaze;
        HMD = CamoForaging.ExperimentController.HMD;
    }


    public void RaycastGaze (EyeData_v2 eyeData) {
            
            
        // raycast
        Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
        else { 
            // Debug.LogWarning("Could not get sranipal unity gaze data.");
            return; 
        }

        var valid_bit_mask = eyeData.verbose_data.combined.eye_data.eye_data_validata_bit_mask;
        // because this is for combined data, pupil diameter, eye openness and pupil position will always be invlaid.
        // therefore, if both gaze origin and direction are valid, the bit mask value will be 3.
        validData = valid_bit_mask == 3;
        if (validData) {
            
            Vector3 GazeOriginCombined = HMD.transform.TransformPoint(GazeOriginCombinedLocal);
            Vector3 GazeDirectionCombined = HMD.transform.TransformDirection(GazeDirectionCombinedLocal);
            validHit = Physics.Raycast(GazeOriginCombined, GazeDirectionCombined, out hit, maxRayLength);
            if (!validHit) {
                // we didn't hit anything. use the gaze ray to create the gaze point, and set hit object to null
                hit.point = HMD.transform.position + GazeDirectionCombined.normalized * maxRayLength;
            }
            
        }

        if (GotGazeRayCallback != null) {
            GotGazeRayCallback.Invoke(hit, validData && validHit, eyeData);
        }
            
    }
}
