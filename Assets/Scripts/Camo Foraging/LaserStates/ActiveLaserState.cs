using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamoForaging.LaserIndicator {
    public class ActiveLaserState : LaserSniperController.LaserState
    {
        private RaycastHit hit;
        private float rayDistance;
        private bool rayHit = false;
        private int noTreeMask = 0;

        public override void EnterState()
        {
            // make sure the laser object is a child of the given hand transform, at local origin
            // lc.laserBeam.transform.SetParent(lc.handTransform);
            // lc.laserBeam.transform.localPosition = Vector3.zero;
            // enable the beam
            noTreeMask = 1 << LayerMask.NameToLayer("Trees");
            noTreeMask = ~noTreeMask;
            lc.laserBeam.enabled = true;
            // listen for trigger pulls
            ParticipantInput.OnTriggerPress += OnTriggerPull;
            lc.laserBeam.SetPositions(new Vector3[] {
                Vector3.zero,
                Vector3.forward * 10f
            });
        }

        public override void Tick()
        {
            if (!lc.Active) {
                SetState(new InactiveLaserState());
            }
            
            lc.laserBeam.transform.position = lc.handTransform.position;
            lc.laserBeam.transform.rotation = lc.handTransform.rotation;
            if (Physics.Raycast(lc.handTransform.position, lc.handTransform.forward, out hit, 130f, noTreeMask)) {
               rayDistance = Vector3.Distance(lc.handTransform.position, hit.point);
            //    lc.laserBeam.enabled = true;
            } else {
                // lc.laserBeam.enabled = false;
            }

            lc.laserBeam.SetPosition(1, Vector3.forward * rayDistance);

            // lc.laserBeam.SetPositions(new Vector3[] {
            //     lc.handTransform.position,
            //     lc.handTransform.forward * rayDistance
            // });
        }

        public override void ExitState()
        {
            if (lc.laserBeam != null) { // getting error when closing unity? because of FSM exit on destroy
                // disable the beam
                lc.laserBeam.enabled = false;
            }
            
            // stop listening for trigger pulls
            ParticipantInput.OnTriggerPress -= OnTriggerPull;
        }

        private void OnTriggerPull(bool leftTriggerPressed) {
            // Debug.Log("no tree layermask: " + noTreeMask);
            rayHit = Physics.Raycast(lc.handTransform.position, lc.handTransform.forward, out hit, lc.maxRayLength, noTreeMask);
            // Debug.Log("Got trigger pull from: " + (leftTriggerPressed ? "Left" : "Right"));
            string actionSubject = "None";
            if (rayHit) {
                actionSubject = hit.collider.name;
                Debug.DrawLine(lc.handTransform.position, hit.point, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 120f);
                Debug.LogFormat("Hit object {0}, which was a {1}",
                    hit.collider.name,
                    hit.collider.name.ToLower().Contains("target") ? "Target" : "Distractor"
                );
                // lc.hitObject = hit.collider.gameObject;
                if (hit.collider.name.ToLower().Contains("target")) {
                    hit.collider.GetComponentInChildren<SoldierAnimation>().GotShot();
                    // hit.collider.gameObject.SetActive(false); // using a pool, so only deactivate.
                    // GameObject.Destroy(hit.collider.gameObject);
                } else if (hit.collider.name.ToLower().Contains("distractor")) {
                    lc.negativeRadioMessage.time = lc.negativeRadioMessageOffset; // with this particular sample,
                    // we need to start a bit earlier in the clip.
                    lc.negativeRadioMessage.Play();
                    hit.collider.GetComponentInChildren<SoldierAnimation>().GotShot();
                    
                }
            }
            lc.sniperRifleShot.Play();
            LSLEventRecorder.RecordParticipantActions("Shoot", actionSubject);
        }
    }

}
