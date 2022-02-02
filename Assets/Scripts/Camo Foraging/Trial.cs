using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trial
{

    // trial needs to have the following properties:
    // Low/High distractor similarity
    // Inter-Target Distance (5, 8, 10)
    // number of targets (12 - 18)
    // static (target is present from beginning of trial) / pop up targets.
    // for now, assume equal number of distractors, as well as even left/right distribution
    public bool useHardDistractors;
    public int numTargets, numDistractors;
    public float spawnCenterDirection, interTargetDistance, delayTargetPresentation;
    public Vector3[] targetPositions, distractorPositions;
}
