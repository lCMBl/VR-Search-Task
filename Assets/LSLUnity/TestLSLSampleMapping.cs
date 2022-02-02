using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLSLSampleMapping : MonoBehaviour
{
    public GameObject testObject;
    public int childThreshold;
    public LSLStreamDefinition sd;
    public float delayTime = 5f;
    private float timer = 0f;
    private bool readyToSend = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // timer += Time.deltaTime;
        // if (readyToSend && timer >= delayTime) {
        //     LSLEventRecorder.RecordTestStreamDefinition(testObject, childThreshold);
        //     List<string> ias = new List<string>();
        //     foreach(var ia in sd.inputArguments) {
        //         ias.Add(string.Format("{0} {1}", ia.type, ia.variableName));
        //     }
        //     Debug.Log(string.Join(", ", ias));
        //     readyToSend = false;
        // }
    }
}
