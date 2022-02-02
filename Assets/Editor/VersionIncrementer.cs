using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class VersionIncrementer : IPostprocessBuildWithReport
{
    // automatically increment the version number after an EXE is built.
    public int callbackOrder => 1;

    public void OnPostprocessBuild(BuildReport report)
    {
        // to update major or minor version, manually set it in Edit>Project Settings>Player>Other Settings>Version
        string[] versionParts = PlayerSettings.bundleVersion.Split('.');
        int build = 0;
        if (versionParts.Length != 3 || !int.TryParse(versionParts[2], out build)) {
            Debug.LogError("BuildPostprocessor failed to update version " + PlayerSettings.bundleVersion);
            return;
        }
        // major-minor-build
        versionParts[2] = (build + 1).ToString();
        PlayerSettings.bundleVersion = string.Join(".", versionParts);
    }
}
