using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ParticipantInput : MonoBehaviour
{
    // For now, debugging, but later want this to be mostly filled with delegates to be subscribed to
    // elsewhere in the program
    // https://sarthakghosh.medium.com/a-complete-guide-to-the-steamvr-2-0-input-system-in-unity-380e3b1b3311

    public SteamVR_Action_Boolean indicateTarget;
    public SteamVR_Action_Boolean nextTrial; // Need to make a new action for progressing to the next trial
    public SteamVR_Input_Sources handType;
    // public LineRenderer laserBeam;
    // public Transform handTransform;
    // private RaycastHit hit;
    // private Vector3 laserEndPosition;

    public static TriggerPressed OnTriggerPress;
    public static TouchpadPressed OnTouchpadPress;

    // public delegate void TriggerPressed(RaycastHit hit, bool valid);
    public delegate void TriggerPressed(bool leftTriggerPressed);
    public delegate void TouchpadPressed(bool leftTouchpadPressed);

    // Start is called before the first frame update
    void Start()
    {
        indicateTarget.AddOnStateDownListener(TriggerDown, handType);
        nextTrial.AddOnStateDownListener(TouchpadDown, handType);
    }

    public void TriggerDown (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        // Debug.Log("Trigger pressed");
        // OnTriggerPress.Invoke(laserBeam.enabled ? hit : new RaycastHit(), laserBeam.enabled);
        if (OnTriggerPress != null) {
            OnTriggerPress.Invoke(fromSource == SteamVR_Input_Sources.LeftHand);
        }
    }

    public void TouchpadDown (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        if (OnTouchpadPress != null) {
            OnTouchpadPress.Invoke(fromSource == SteamVR_Input_Sources.LeftHand);
        }
    }

    void OnDestroy() {
        indicateTarget.RemoveAllListeners(handType);
        nextTrial.RemoveAllListeners(handType);
        OnTriggerPress = null;
        OnTouchpadPress = null;
    }
}
