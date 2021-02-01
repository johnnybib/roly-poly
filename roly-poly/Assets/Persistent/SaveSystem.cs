using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    private static string filename = "/savefile.dat";

    public static void SavePlayerControllerData(PlayerController player)
    {
        SaveData saveData = new SaveData();
        saveData.dribbleUnlock = player.abilities.abilities.dribble.unlocked;
        saveData.stickyFeetUnlock = player.abilities.abilities.stickyFeet.unlocked;
        saveData.boostBallUnlock = player.abilities.abilities.boostBall.unlocked;
        saveData.bugBlastUnlock = player.abilities.abilities.bugBlast.unlocked;
        saveData.savePositionX = player.physics.transform.position.x;
        saveData.savePositionY = player.physics.transform.position.y;
        saveData.savePositionZ = player.physics.transform.position.z;

        SaveSystem.SaveFile(saveData);
    }
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
