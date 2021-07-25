using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataMaster : MonoBehaviour
{
    public static PlayerDataMaster Instance;
    
    public PlayerData currentPlayerData;
    
    public GameObject loadedSavePrefab;

    //string saveFolderPath = Application.dataPath + "Saves/";
    string saveFolderPath;
    //string saveFileSuffix = "savedgame_"; 
    string saveFileSuffix; // + currentPlayerData.playerName. add this after load.

    List<string> saveNameList;
    [SerializeField]
    private string defualtName;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        saveFolderPath = Application.dataPath + "/Resources/Saves/";
        saveFileSuffix = "savedgame_";

        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Save Me")]
    public void SaveDataToDisk()
    {
        //check folder and file
        if(currentPlayerData.playerName.Length == 0 || currentPlayerData.playerName == null)
        {
            currentPlayerData.playerName = defualtName;
        }
        if(!CheckForSaveFolderAndFile())
        {
           File.Create(saveFolderPath + saveFileSuffix + currentPlayerData.playerName + ".txt").Close(); //create file if none exists.
        }

        //assume: a new file was created at "saveFolderPath + saveFileSuffix" 
        //dont assume - player has set a name yet. this should work even if they somehow manage/are-allowed not to set their own profile


        string pdJsonString = JsonUtility.ToJson(currentPlayerData);

        File.WriteAllText(saveFolderPath + saveFileSuffix + currentPlayerData.playerName + ".txt", pdJsonString);

        //consider overwriting 

        //try save

        //save successful messege
    }
    public void LoadDataFromDisk(string dirToSaveFile)
    {
        //check folder and file
        if (!CheckForSaveFolderAndFile())
        {
            Debug.LogError("no such save-file/folder");
            //return;
        }

        //assume: a new file was created at "saveFolderPath + saveFileSuffix" 
        //dont assume - player has set a name yet. this should work even if they somehow manage/are-allowed not to set their own profile
        string contents = File.ReadAllText(saveFolderPath + saveFileSuffix);

        PlayerData pd = JsonUtility.FromJson<PlayerData>(contents);

        if(pd != null)
        {
            LoadPlayerData(pd);
        }

        //consider overwriting 

        //try save

        //save successful messege
    }
    void LoadPlayerData(PlayerData pd)
    {
        currentPlayerData = pd;
    }
    bool CheckForSaveFolderAndFile()
    {
        if(!Directory.Exists(saveFolderPath))
        {
            Debug.LogWarning("No folder found in save-folder-path");

            Directory.CreateDirectory(saveFolderPath);

            Debug.LogWarning("Save folder created at: " + saveFolderPath);
        }

        string[] files = Directory.GetFiles(saveFolderPath);

        if (files.Length == 0) //must be the case if folder is new
        {
            //File.Create(saveFolderPath + saveFileSuffix + currentPlayerData.playerName);
            Debug.Log("no save files found");
            return false;
        }

        // Asserted: folder exists, and it contains files

        //foreach (string s in files)
        //{
        //    if(s.Contains(saveFileSuffix))
        //    {
        //        //display saved profiles
        //        saveNameList.Add(s);
        //    }
        //}
        return true;
    }

    List<string> GetConfirmedSaveFileNames() //in the save-folder (default: "saves")
    {
        string[] files = Directory.GetFiles(saveFolderPath);

        if (files.Length == 0) //must be the case if folder is new
        {
            //File.Create(saveFolderPath + saveFileSuffix + currentPlayerData.playerName);
            Debug.Log("no save files found");
            return null;
        }
        //asserted: files exist in folder

        //are they really save files? (do they contain the string "savegame" in the file name?
        //Either check for "appropriate" entries in the list and add them to a new list (List<string> confirmedSaveFileNames

        //List<string> confirmedSaveFileNames = new List<string>();

        List<string> confirmedSaveFileNames = files.ToList(); //save names to a new list 
        confirmedSaveFileNames.RemoveAll(x => !x.Contains(saveFileSuffix) || x.Contains("meta")); //removes all entries which do not contain "savegame_" in their filename

        if(confirmedSaveFileNames.Count == 0)
        {
            Debug.Log("Save files either do not exist, or for whatever reason could not be confirmed - make sure that the Contains(saveFileSuffix) check is actually checking for a string that is contained within ANY savefile for this game");
            return null;
        }

        return confirmedSaveFileNames;
    }

    void DisplaySaveFiles(List<string> confirmedFileNames)
    {
        //load up
    }

    public PlayerData[] GetPlayerDataFromSaveList()
    {
        List<string> confirmedList = GetConfirmedSaveFileNames();

        PlayerData[] playerDatas = new PlayerData[confirmedList.Count];
        for (int i = 0; i < confirmedList.Count; i++)
        {
            string s = File.ReadAllText(confirmedList[i]);

            //if (s.Length > 2)
            //{
            PlayerData p = JsonUtility.FromJson<PlayerData>(s);

            playerDatas[i] = p;
            //}


            //str.Close();
        }

        return playerDatas;
    }
}
