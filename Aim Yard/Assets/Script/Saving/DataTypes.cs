using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Data
{
    public static readonly string folderpath = Application.persistentDataPath + "/data";

    public void ValidateDirectory()
    {
        if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
    }
}

public class SettingsData : Data
{
    public int[] resolution;
    public int qualityLevel;
    public int displayMode;
    public string filepath = folderpath + "/settings.bin";

    public SettingsData(int[] _resolution, int _qualityLevel, int _displayMode)
    {
        resolution = _resolution;
        qualityLevel = _qualityLevel;
        displayMode = _displayMode;
        ValidateDirectory();
    }
}

public class WeaponData : Data
{
    public string prefabName;
    public string filepath = folderpath + "/curgun.bin";

    public WeaponData(string _prefabName)
    {
        prefabName = _prefabName;
        ValidateDirectory();
    }

}