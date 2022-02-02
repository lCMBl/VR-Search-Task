using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    private static ProgressBar instance;

    public Slider barSlider;
    public Text barText;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static void UpdateBar(int currentTrial, int numberOfTrials) {
        instance.barSlider.maxValue = numberOfTrials;
        instance.barSlider.value = currentTrial;
        instance.barText.text = currentTrial + " / " + numberOfTrials;
    }
}


