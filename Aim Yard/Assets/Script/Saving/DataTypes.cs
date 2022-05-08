using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

/*
 * All .bin saved data inherits the Data class for a common folder path
 * 
 * Child data classes can be defined and given their own variables
 * (which get filled on construction if needed)
 * 
 */


[System.Serializable]
public class Data
{
    public static readonly string folderpath = Application.persistentDataPath + "/data";

    public void ValidateDirectory()
    {
        // Check if the directory exists. if not, make the directory.
        // This prevents any accessing errors we may come accross (ie, game started for the first time)
        if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
    }
}

public class SettingsData : Data
{
    // variable names should be descriptive enough here
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
    // We only save the prefab name as the name will be used to access and load in the prefab
    public string prefabName;
    public string filepath = folderpath + "/curweapon.bin";

    public WeaponData(string _prefabName)
    {
        prefabName = _prefabName;
        ValidateDirectory();
    }

}