using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Save system class saves .bin files to the application's persistent data path (which keeps the 
 * path consistent across all devices). 
 * 
 * Save functions are polymorphic to allow easy use in other functions.
 * 
 * Load functions have to be explicit in order to target the right files.
 */
static public class SaveSystem
{
    public static void SaveData (SettingsData settings)
    {
        // Creates binary formatter for the .bin creation.
        BinaryFormatter formatter = new BinaryFormatter();
        // Uses filestream to allow saving to our requested path,
        // in this case our filepath is defined in our settings object (see DataTypes.cs)
        FileStream stream = new FileStream(settings.filepath, FileMode.Create);

        formatter.Serialize(stream, settings);
        // Important to close the file stream after use
        stream.Close();
    }

    public static void SaveData (WeaponData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(data.filepath, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettings()
    {
        // Target the path specified in our settings data type
        string path = Application.persistentDataPath + "/data/settings.bin";
        // check if path exists. this includes if the file exists.
        if (File.Exists(path))
        {
            // Formatter to read the file, file stream to get the file.
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // Convert our bin into a SettingsData variable
            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();
            
            // Return data
            return data;
        } else
        {
            return null;
        }
    }

    public static WeaponData LoadGunData()
    {
        string path = Application.persistentDataPath + "/data/curweapon.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            WeaponData data = formatter.Deserialize(stream) as WeaponData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
