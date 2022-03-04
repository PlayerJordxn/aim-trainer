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

    public static void SaveData(GunData data)
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

    public static GunData LoadGunData()
    {
        string path = Application.persistentDataPath + "/data/curgun.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GunData data = formatter.Deserialize(stream) as GunData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
