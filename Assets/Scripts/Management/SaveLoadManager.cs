using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class SaveLoadManager
{
    private const string _baseFolderName = "BoxSlidingDoorData";


    /// <summary>
    /// Determines the save path to use when loading and saving a file based on a folder name.
    /// </summary>
    /// <returns>The save path.</returns>
    /// <param name="folderName">Folder name.</param>
    static string DetermineSavePath(string _fileName)
    {
        string savePath;
        string sceneName = SceneManager.GetActiveScene().name;

        // depending on the device we're on, we assemble the path
        savePath = Path.Combine(Application.persistentDataPath, _baseFolderName, DataManager.participantNumber.ToString(), sceneName);

        //#if UNITY_EDITOR
        //savePath = Path.Combine(Application.dataPath, _baseFolderName);
        //#endif

        //savePath += "/";
        //savePath = Path.Combine(Application.persistentDataPath, _baseFolderName, _fileName);
        return savePath;
    }

    public static void Save(object saveObject, string fileName)
    {
        string savePath = DetermineSavePath(fileName);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            //Debug.Log(savePath);
        }
        string jsonDataString = JsonUtility.ToJson(saveObject);
        File.WriteAllText(savePath + "/" + fileName + ".json", jsonDataString);

    }
}
