using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

static public class SaveSystem
{
    public static void SaveData (SettingsData settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(settings.filepath, FileMode.Create);

        formatter.Serialize(stream, settings);
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
        string path = Application.persistentDataPath + "/data/settings.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

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
