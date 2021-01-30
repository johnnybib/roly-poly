using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    private static string filename = "/savefile.dat";
    public static void SaveFile(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + filename;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadFile()
    {
        string path = Application.persistentDataPath + filename;
        if (!File.Exists(path))
        { // File not found.
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        SaveData data = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        return data;
    }

    public static bool DoesSaveFileExist()
    {
        string path = Application.persistentDataPath + filename;
        return File.Exists(path);
    }
}
