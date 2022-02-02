using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;

[RequireComponent(typeof(LineRenderer))]
public class ViveProEyeGazeVisualizer : MonoBehaviour
{
    public float lineLength;
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private bool running;
    [SerializeField]
    private Transform hmd;

    public bool Running { 
        get { return running; }
        set { 
            running = value;
            lr.enabled = value;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.enabled = running;
        hmd = CamoForaging.ExperimentController.HMD;
        ViveProEyeTracker.GotEyeDataCallback += UpdateGazeRay;
    }

    void UpdateGazeRay(EyeData_v2 eyeData) {
        
        if (running) {    
            // raycast
            Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
            if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
            else { 
                return; 
            }

            var valid_bit_mask = eyeData.verbose_data.combined.eye_data.eye_data_validata_bit_mask; // 3 for all valid combined data
            if (valid_bit_mask == 3) {
                Vector3 GazeOriginCombined = hmd.transform.TransformPoint(GazeOriginCombinedLocal);
                Vector3 GazeDirectionCombined = hmd.transform.TransformDirection(GazeDirectionCombinedLocal);
                lr.SetPosition(0, GazeOriginCombined + GazeDirectionCombined.normalized); // add direction to push the line start away from head
                lr.SetPosition(1, (GazeDirectionCombined * lineLength) + GazeOriginCombined);
            }
        }
    }

    void OnDestroy() {
        Debug.Log("Removed gaze ray from VPET");
        ViveProEyeTracker.GotEyeDataCallback -= UpdateGazeRay;
    }
}
