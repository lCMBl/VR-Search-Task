using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlockDescription", menuName = "CamoForaging/Block Description", order = 2)]
public class BlockDescription : ScriptableObject
{
    // used to generate the trials for a block / experimental run.
    /*
    public bool useHardDistractors, delayTargetPresentation;
    public int numTargets, numDistractors;
    public float spawnCenterDirection, interTargetDistance;
    */
    public int numRepeats;
    public float[] interTargetDistances;
    public int minNumTargets, maxNumTargets, numDistractors;
    public float centerPointOffset; // how far in degrees center spawn directions should be away from one another.


    public void GenerateTrials(MonoBehaviour host) {
        host.StartCoroutine(GenerateAndStoreTrials());
    }

    private IEnumerator GenerateAndStoreTrials() {
        List<Trial> results = new List<Trial>();
        int frameCount = 0;
        for (int i = 0; i < numRepeats; i++) {
            for (int numTargets = minNumTargets; numTargets <= maxNumTargets; numTargets++) {
                foreach(var itd in interTargetDistances) {
                    foreach(var useHardDistractors in new bool[] {false, true}) {
                        var t = new Trial();
                        t.useHardDistractors = useHardDistractors;
                        t.numTargets = numTargets;
                        t.numDistractors = numDistractors;
                        t.interTargetDistance = itd;
                        results.Add(t);
                        frameCount++;
                        if (frameCount >= 20) {
                            yield return new WaitForEndOfFrame();
                            frameCount = 0;
                        }
                    }
                }
            }
        }

        // now that all trials have been made, shuffle, then chose appropriate center points.
        // TODO this will have to be done later, to account for participant's current look direction
        results.Shuffle();
        // float prevCenter = Random.Range(-180f, 180f);
        // foreach(var t in results) {
        //     float center = prevCenter;
        //     do {
        //         center += Random.Range(-180f, 180f);
        //         frameCount++;
        //         if (frameCount >= 20) {
        //             yield return new WaitForEndOfFrame();
        //             frameCount = 0;
        //         }
        //     } while(Mathf.Abs(center-prevCenter) < centerPointOffset );
            
        //     t.spawnCenterDirection = center;
        //     prevCenter = center;
        // }

        // here is where trials need to be returned to a central point
        CamoForaging.ExperimentController.Trials = results.ToArray();
        CamoForaging.ExperimentController.SetFirstTrial();
    }
}

public static class ListExtension {
        private static System.Random rng = new System.Random(1992);  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
