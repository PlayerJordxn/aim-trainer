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

public class GunData : Data
{
    public string gunName;
    public string gunSourceAudioFilePath;
    public string gunClipAudioFilePath;
    public string gunMuzzleFlashFilePath;
    public string filepath = folderpath + "/curgun.bin";

    public GunData(string _gunName, string _gunAudioSourceFP, string _gunClipAudioFP, string _gunMuzzleFP)
    {
        gunName = _gunName;
        gunSourceAudioFilePath = _gunAudioSourceFP;
        gunClipAudioFilePath = _gunClipAudioFP;
        gunMuzzleFlashFilePath = _gunMuzzleFP;
        Debug.Log(gunSourceAudioFilePath);
        ValidateDirectory();
    }

}