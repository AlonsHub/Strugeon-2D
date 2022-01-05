using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Google.Apis;

public class PlayerDataMaster : MonoBehaviour
{
    public static PlayerDataMaster Instance;

    public PlayerData currentPlayerData;

    public GameObject loadedSavePrefab;

    //string saveFolderPath = Application.dataPath + "Saves/";
    string saveFolderPath;
    //string saveFileSuffix = "savedgame_"; 
    string saveFilePrefix; // + currentPlayerData.playerName. add this after load.

    List<string> saveNameList;
    [SerializeField]
    private string defualtName = "Psion";

    public int RoomCount { get => currentPlayerData.rooms.Count; }
    public int MaxRoomCount { get => 4; } //temp
    public Dictionary<string, float> SavedCooldowns { get => currentPlayerData.SiteCooldownTimes; private set => currentPlayerData.SiteCooldownTimes = value; } //to be read from and to

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        saveFolderPath = Application.dataPath + "/Resources/Saves/";
        saveFilePrefix = "savedgame_";

        //SavedCooldowns = new Dictionary<string, float>();

        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Save Me")]
    public void SaveDataToDisk()
    {
        //check folder and file
        if (currentPlayerData.playerName.Length == 0 || currentPlayerData.playerName == null)
        {
            currentPlayerData.playerName = defualtName;
        }
        if (!CheckForSaveFolder()) //creates folder if there is none - and returns false if (folder doesn't exists) (files don't exist)
        {
            File.Create(saveFolderPath + saveFilePrefix + currentPlayerData.playerName + ".txt").Close(); //create file if none exists.
        }
        //else
        //{
        //    if (SaveExistsCheck(currentPlayerData.playerName) != null)
        //    {
        //        File.WriteAllText(saveFolderPath + saveFilePrefix + currentPlayerData.playerName + ".txt", );
        //    }
        //}

        //assume: a new file was created at "saveFolderPath + saveFileSuffix" 
        //dont assume - player has set a name yet. this should work even if they somehow manage/are-allowed not to set their own profile

        currentPlayerData.values = currentPlayerData.SiteCooldownTimes.Values.ToList();
        currentPlayerData.keys = currentPlayerData.SiteCooldownTimes.Keys.ToList();

        string pdJsonString = JsonUtility.ToJson(currentPlayerData);

        //kill any 0 that follows a 0

        //currentPlayerData.squadsAsMercNameList.

        File.WriteAllText(saveFolderPath + saveFilePrefix + currentPlayerData.playerName + ".txt", pdJsonString);

        if(GoogleSheetMaster.Instance)
        GoogleSheetMaster.Instance.LogPlayer();

        //consider overwriting 

        //try save

        //save successful messege
    }

    //void KillFollowingZeros(ref List<int> nums)
    //{
    //    for (int i = 0; i < nums.Count; i++)
    //    {
    //        if(nums[i]==0 && nums[i+1] == 0)
    //        {
    //            int k = 1;
    //            while(nums[i+k]==0)
    //            {
    //                k++;
    //            }
    //        }
    //    }
    //}

    public void LoadDataFromDisk(string saveName)
    {
        //check folder and file
        if (!CheckForSaveFolder())
        {
            Debug.LogError("no such save-file/folder");
            //return;
        }

        //assume: a new file was created at "saveFolderPath + saveFileSuffix" 
        //dont assume - player has set a name yet. this should work even if they somehow manage/are-allowed not to set their own profile
        string contents = File.ReadAllText(saveFolderPath + saveFilePrefix + saveName + ".txt");

        PlayerData pd = JsonUtility.FromJson<PlayerData>(contents);

        if (pd != null)
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

        currentPlayerData.SiteCooldownTimes = new Dictionary<string, float>();
        if (currentPlayerData.values == null && currentPlayerData.keys == null)
        {
            return;
        }
        if (currentPlayerData.values.Count != currentPlayerData.keys.Count)
        {
            Debug.LogError("key and value amounts don't match");
        }

        for (int i = 0; i < currentPlayerData.values.Count; i++)
        {
            currentPlayerData.SiteCooldownTimes.Add(currentPlayerData.keys[i], currentPlayerData.values[i]);
        }


        //PARTY REF?

        //REFMASTER

        //INVENTORY

        //Load avail mercs into the ref master and the party master
        // dont forget to check

    }
    bool CheckForSaveFolder() //ensures that a save folder exists and there are files in it to load
    {
        if (!Directory.Exists(saveFolderPath))
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

        //not empty, now - is there a save file for this player


        return true;
    }
    bool CheckForSaveFolder(out string[] containedFiles) //ensures that a save folder exists and there are files in it to load
    {
        containedFiles = new string[] { "" };
        if (!Directory.Exists(saveFolderPath))
        {
            Debug.LogWarning("No folder found in save-folder-path");

            Directory.CreateDirectory(saveFolderPath);

            Debug.LogWarning("Save folder created at: " + saveFolderPath);
        }

        containedFiles = Directory.GetFiles(saveFolderPath);

        if (containedFiles.Length == 0) //must be the case if folder is new
        {
            //File.Create(saveFolderPath + saveFileSuffix + currentPlayerData.playerName);


            Debug.Log("no save files found");
            return false;
        }

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
        confirmedSaveFileNames.RemoveAll(x => !x.Contains(saveFilePrefix) || x.Contains("meta")); //removes all entries which do not contain "savegame_" in their filename

        if (confirmedSaveFileNames.Count == 0)
        {
            Debug.Log("Save files either do not exist, or for whatever reason could not be confirmed - make sure that the Contains(saveFileSuffix) check is actually checking for a string that is contained within ANY savefile for this game");
            return null;
        }

        return confirmedSaveFileNames;
    }
    public PlayerData[] GetPlayerDataFromSaveList()
    {
        List<string> confirmedList = GetConfirmedSaveFileNames();

        PlayerData[] playerDatas = new PlayerData[confirmedList.Count];
        for (int i = 0; i < confirmedList.Count; i++)
        {
            string s = File.ReadAllText(confirmedList[i]);

            PlayerData p = JsonUtility.FromJson<PlayerData>(s);

            playerDatas[i] = p;
        }

        return playerDatas;
    }

    public void CreateNewSave(string newPlayerName)
    {
        PlayerData newPD = new PlayerData();

        newPD.playerName = newPlayerName;

        //Get starting mercs
        newPD.availableMercs = new List<MercName>();
        newPD.availableMercs.AddRange(GameStats.startMercNames);
        newPD.gold = GameStats.startingGold;
        newPD.rooms = new List<Room>();
        newPD.rooms.Add(new Room());
        //newPD.squadsAsMercNames = new List<List<MercName>>();


        //squads should be empty

        LoadPlayerData(newPD);

        SaveDataToDisk();
    }

    public void GrabAndSaveData()
    {
        List<MercName> newNames = new List<MercName>();

        foreach (var merc in PartyMaster.Instance.availableMercs)
        {
            newNames.Add(merc.mercName);
        }
        currentPlayerData.availableMercs = newNames;


        currentPlayerData.squadsAsMercNameList = GetSquadsList();


        SaveDataToDisk();
    }

    public List<MercName> GetSquadsList()
    {
        List<MercName> toReturn = new List<MercName>();

        foreach (Room s in Instance.currentPlayerData.rooms) //sets squads in order of rooms!!!!!!
        {
            toReturn.Add(MercName.None);
            if (s!=null && s.isOccupied)
            {
                foreach (Pawn p in s.squad.pawns)
                {
                    toReturn.Add(p.mercName);
                }
            }
        }

        return toReturn;
    }

    public string SaveExistsCheck(string playerName)
    {
        string[] files;
        if (!CheckForSaveFolder(out files))
            return null; //false

        //folder has files in it, and will be "outted" by CheckForSaveFolder(out files), 
        //which gets its files from CheckSaveFolder(out string[] files)

        foreach (string s in files)
        {
            if (s.Contains(saveFilePrefix + currentPlayerData.playerName))
            {
                return s;
            }
        }

        return null; //false
    }
    public void AddHireableMerc(MercName mercName)
    {
        if (currentPlayerData.hireableMercs == null)
        {
            currentPlayerData.hireableMercs = new List<MercName>();
        }
        else if (currentPlayerData.hireableMercs.Contains(mercName))
        {
            Debug.LogWarning("You have this merc already");
            return;
        }
        currentPlayerData.hireableMercs.Add(mercName);
    }
    private void OnApplicationQuit()
    {

    }

    public void HireMerc(MercName mn)
    {
        PartyMaster.Instance.availableMercs.Add(MercPrefabs.Instance.EnumToPawnPrefab(mn));
        currentPlayerData.hireableMercs.Remove(mn);
        currentPlayerData.availableMercs.Add(mn);

        //GoogleSheetMaster.Instance.LogPlayer(); //saves these changes
    }

    public List<System.Object> GetLog
    {
        get => new List<System.Object> { currentPlayerData.playerName,
        currentPlayerData.victories,
        currentPlayerData.losses,
        currentPlayerData.numOfavailableMercs,
        currentPlayerData.deadMercs,
        currentPlayerData.cowardMercs,
        currentPlayerData.gold};
    }
}
