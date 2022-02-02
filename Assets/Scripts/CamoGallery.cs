using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamoGallery : MonoBehaviour
{
    // one off script to create sample galleries of characters in different camo patterns
    public CamoGalleryConfig config;
    public Transform galleryTopLeftPosition;
    public float verticalSpacing, horizontalSpacing;
    public GameObject distanceCallout;
    public string[] camoChannelNames;
    public string filepath;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogFormat("{0}", config.mainCamoPattern.GetColor("_CamoBlackTint").ToString());
        // CreateGalleryCharacter(config.camoPatterns[0], 1, Vector3.zero);
        StartCoroutine(BuildAllGalleries());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCamoPatternColors(string colorChannel, Material sourceCamo) {
        foreach (var cm in config.camoPatterns) {
            cm.SetColor(colorChannel, sourceCamo.GetColor(colorChannel));
        }
    }

    private GameObject CreateGalleryCharacter(Material camoPattern, int poseID, Vector3 position) {
        GameObject go = Instantiate(config.characterPrefab, position, Quaternion.Euler(0f, 180f, 0f));
        foreach(var path in config.camoTransformPaths) {
            // Debug.Log(path);
            // Debug.Log(go.transform.Find(path));
            go.transform.Find(path).GetComponent<SkinnedMeshRenderer>().material = camoPattern;
        }
        go.GetComponentInChildren<Animator>().SetInteger("poseID", poseID);
        go.GetComponentInChildren<Animator>().SetTrigger("SetPose");
        go.transform.SetParent(galleryTopLeftPosition);
        //.Sample();
        return go;
    }

    public void GenerateGallery(CamoGalleryConfig cgc, int primaryCamoIndex) {
        // generate top row (poses)
        // first in row is always the main camo character
        CreateGalleryCharacter(cgc.mainCamoPattern, 0, galleryTopLeftPosition.position);

        for (int i = 0; i < cgc.poseCount; i++) {
            CreateGalleryCharacter(
                cgc.camoPatterns[primaryCamoIndex],
                i,
                galleryTopLeftPosition.position + (Vector3.right * horizontalSpacing * (i + 1))
            );
        }

        CreateGalleryCharacter(cgc.mainCamoPattern, 0, galleryTopLeftPosition.position + (Vector3.up * -verticalSpacing));

        // generate middle row (distances)
        for (int j = 0; j < cgc.distances.Length; j++) {
            var go = CreateGalleryCharacter(
                cgc.camoPatterns[primaryCamoIndex],
                0,
                galleryTopLeftPosition.position + (Vector3.up * -verticalSpacing) + (Vector3.right * horizontalSpacing * (j+1))
            );
            GameObject callout = Instantiate(distanceCallout, go.transform.position, Quaternion.identity);
            callout.transform.SetParent(galleryTopLeftPosition);
            // callout.transform.LookAt(transform, Vector3.up);
            go.transform.LookAt(transform, Vector3.up);
            go.transform.position += go.transform.forward * -cgc.distances[j];
            callout.GetComponentInChildren<Text>().text = Vector3.Distance(go.transform.position, transform.position).ToString() + " meters";
        }

        CreateGalleryCharacter(cgc.mainCamoPattern, 0, galleryTopLeftPosition.position + (Vector3.up * -verticalSpacing * 2));

        // generate bottom row (camo)
        for (int k = 0; k < cgc.distances.Length; k++) {
            CreateGalleryCharacter(
                cgc.camoPatterns[k],
                0,
                galleryTopLeftPosition.position + (Vector3.right * horizontalSpacing * (k+1)) + (Vector3.up * -verticalSpacing * 2)
            );
        }
    }

    // public void GenerateGallery(CamoGalleryConfig cgc, int primaryCamoIndex) {

        
    //     // generate top row (poses)
    //     for (int i = 0; i < cgc.poseCount; i++) {
    //         CreateGalleryCharacter(
    //             cgc.camoPatterns[primaryCamoIndex],
    //             i,
    //             galleryTopLeftPosition.position + (Vector3.right * horizontalSpacing * i)
    //         );
    //     }

    //     // generate middle row (distances)
    //     for (int j = 0; j < cgc.distances.Length; j++) {
    //         var go = CreateGalleryCharacter(
    //             cgc.camoPatterns[primaryCamoIndex],
    //             0,
    //             galleryTopLeftPosition.position + (Vector3.up * -verticalSpacing) + (Vector3.right * horizontalSpacing * j)
    //         );
    //         go.transform.LookAt(transform, Vector3.up);
    //         go.transform.position += go.transform.forward * -cgc.distances[j];
    //     }

    //     // generate bottom row (camo)
    //     for (int k = 0; k < cgc.distances.Length; k++) {
    //         CreateGalleryCharacter(
    //             cgc.camoPatterns[k],
    //             0,
    //             galleryTopLeftPosition.position + (Vector3.right * horizontalSpacing * k) + (Vector3.up * -verticalSpacing * 2)
    //         );
    //     }
    // }

    IEnumerator BuildAllGalleries() {
        // reset all colors
        for (int i = 0; i < camoChannelNames.Length; i++) {
            SetCamoPatternColors(camoChannelNames[i], config.mainCamoPattern);
        }
        // build each gallery in turn, changing one color at a time.
        for (int i = 0; i < camoChannelNames.Length; i++) {
            SetCamoPatternColors(camoChannelNames[i], config.distractorCamoPattern);
            // generate the gallery
            GenerateGallery(config, 0);
            // wait for the delay time to let animations play out.
            yield return new WaitForSeconds(2f);

            // take a screenshot
            ScreenCapture.CaptureScreenshot(filepath +  config.distractorCamoPattern.name + "_CamoColorChannel_"+ i + ".png");
            // wait for the end of the frame
            yield return new WaitForEndOfFrame();
            // destroy all current gallery characters
            foreach (Transform gc in galleryTopLeftPosition) {
                Destroy(gc.gameObject);
            }
        }
        // for (int i = 0; i < config.camoPatterns.Length; i++) {
        //     // generate the gallery
        //     GenerateGallery(config, i);
        //     // wait for the delay time to let animations play out.
        //     yield return new WaitForSeconds(2f);
        //     // take a screenshot
        //     ScreenCapture.CaptureScreenshot(filepath + config.camoPatterns[i].name + ".png");
        //     // wait for the end of the frame
        //     yield return new WaitForEndOfFrame();
        //     // destroy all current gallery characters
        //     foreach (Transform gc in galleryTopLeftPosition) {
        //         Destroy(gc.gameObject);
        //     }
        // }
    }
}
