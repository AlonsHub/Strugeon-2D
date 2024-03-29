﻿using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Google.Apis;
using System;

public class PlayerDataMaster : MonoBehaviour
{
    public static PlayerDataMaster Instance;

    public PlayerData currentPlayerData;


    
    //string saveFolderPath = ;
    string saveFolderPath; // VALUE SET ON AWAKE to: Application.dataPath + "/Resources/Saves/"
    string saveFilePrefix = "savedgame_"; // + currentPlayerData.playerName. add this after load.

    List<string> saveNameList;
    [SerializeField]
    private string defualtName = "Psion";

    public int RoomCount { get => currentPlayerData.rooms.Count; }
    public int MaxRoomCount { get => 4; } //temp
    public Dictionary<string, float> SavedCooldowns { get => currentPlayerData.SiteCooldownTimes; private set => currentPlayerData.SiteCooldownTimes = value; } //to be read from and to
    public Dictionary<string, DateTime?> SiteCooldowns { get => currentPlayerData._siteCooldowns; private set => currentPlayerData._siteCooldowns = value; } //to be read from and to

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        saveFolderPath = Application.dataPath + "/Resources/Saves/";

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
        
        currentPlayerData.SaveCooldownsToLists();

        string pdJsonString = JsonUtility.ToJson(currentPlayerData);


        File.WriteAllText(saveFolderPath + saveFilePrefix + currentPlayerData.playerName + ".txt", pdJsonString);

        if(GoogleSheetMaster.Instance)
        GoogleSheetMaster.Instance.LogPlayer();

        //consider overwriting 

        //try save

        //save successful messege
    }
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

        //currentPlayerData.SiteCooldownTimes = new Dictionary<string, float>();
        if (currentPlayerData.siteCooldowns == null && currentPlayerData.siteNames == null)
        {
            return;
        }
        if (currentPlayerData.siteCooldowns.Count != currentPlayerData.siteNames.Count)
        {
            Debug.LogError("key and value amounts don't match");
        }

        for (int i = 0; i < currentPlayerData.siteCooldowns.Count; i++)
        {
            if (currentPlayerData.siteCooldowns[i] == "nocooldown")
                ClearSiteCooldown(currentPlayerData.siteNames[i]);
            else
                AddSiteCooldown(currentPlayerData.siteNames[i], DateTime.Parse(currentPlayerData.siteCooldowns[i]));
        }

        foreach (var merc in currentPlayerData.mercSheets)
        {
            if (merc.gearStrings == null)
                continue;
            merc.gear = new Gear(merc.gearStrings[0], merc.gearStrings[1], merc.gearStrings[2]);
        }
        //if(currentPlayerData.squadSitesAndTimesRemainning.Count > 0)
        //{

        //}
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

        if (playerDatas == null || playerDatas.Length == 0)
            return null;

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
        //newPD.availableMercs = new List<MercName>(); //NEVER DO THIS! WHY INIT A LIST REMOTELY?! SHOULD ALL LISTS BE PRIVATE?! THIS IS A RULE NOW!
        //newPD.availableMercs.AddRange(GameStats.startMercNames);

        newPD.CreateAddMercs(GameStats.startMercNames, MercAssignment.Available);

        newPD.gold = GameStats.startingGold; //THESE SHOULD ALL be Init()
        newPD.rooms = new List<Room>();//THESE SHOULD ALL be Init()
        newPD.rooms.Add(new Room(0));//THESE SHOULD ALL be Init()



        //squads should be empty
        //take merc sheets from available mercs only? or use one method for a sweep-collection?
        LoadPlayerData(newPD);

        SaveDataToDisk();
    }
    [ContextMenu("Grab and save data")]
    public void GrabAndSaveData()
    {
        List<MercName> newNames = new List<MercName>();

        foreach (var merc in currentPlayerData.mercSheets)
        {
            newNames.Add(merc.characterName); //TBF! make mercName a MercSheet property
        }
        currentPlayerData.availableMercNames = newNames; //TBF to remove! only sheets are important now


        currentPlayerData.squadsAsMercNameList = GetSquadsList(); //being phased out TBF 


        foreach (var merc in currentPlayerData.mercSheets)
        {
            merc.SaveGearToString();
        }
        

        //currentPlayerData.mercSheets

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
    public int GetTotalMercLevel()
    {
        List<MercSheet> mercSheets = GetMercSheetsByAssignments(new List<MercAssignment> { MercAssignment.Available, MercAssignment.AwaySquad, MercAssignment.Room });

        int toReturn = 0;
        foreach (var m in mercSheets)
        {
            toReturn += m._level;
        }
        return toReturn;
    }
    public List<MercName> GetMercNamesByAssignments(List<MercAssignment> mercAssignments)
    {
        List<MercName> toReturn = new List<MercName>();
        List<MercSheet> relevantMercs = currentPlayerData.mercSheets.Where(x => mercAssignments.Contains(x.currentAssignment)).ToList();
        foreach (var item in relevantMercs)
        {
            toReturn.Add(item.characterName);
        }
        return toReturn;
    }
    public MercSheet GetMercSheetByName(MercName byName)
    {
        return currentPlayerData.mercSheets.Where(x => x.characterName == byName).SingleOrDefault();
    }
     public MercSheet GetMercSheetByIndex(int ind) //mostly useless
    {
        if (ind >= currentPlayerData.mercSheets.Count)
            return null;

        return currentPlayerData.mercSheets[ind];
    }

    public List<MercName> GetMercNamesByAssignment(MercAssignment mercAssignments)
    {
        List<MercName> toReturn = new List<MercName>();
        List<MercSheet> relevantMercs = currentPlayerData.mercSheets.Where(x => mercAssignments==x.currentAssignment).ToList();
        foreach (var item in relevantMercs)
        {
            toReturn.Add(item.characterName);
        }
        return toReturn;
    }
    public List<MercSheet> GetMercSheetsByAssignments(List<MercAssignment> mercAssignments)
    {
        return currentPlayerData.mercSheets.Where(x => mercAssignments.Contains(x.currentAssignment)).ToList();
    }
    public int GetAmountOfMercSheetsByAssignments(List<MercAssignment> mercAssignments)
    {
        return currentPlayerData.mercSheets.Where(x => mercAssignments.Contains(x.currentAssignment)).ToList().Count;
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
        if (currentPlayerData.hireableMercs.Contains(mercName))
        {
            Debug.LogWarning("You have this merc already");
            return;
        }
        currentPlayerData.CreateAddMercs(new List<MercName> { mercName }, MercAssignment.Hireable);
        //currentPlayerData.hireableMercs.Add(mercName);
    }
    private void OnApplicationQuit()
    {
        GrabAndSaveData();
    }

    public void HireMerc(MercName mn)
    {
        currentPlayerData.hireableMercs.Remove(mn);

        //currentPlayerData.availableMercs.Add(mn);
        //currentPlayerData.CreateAddMercs(new List<MercName>{mn}, MercAssignment.Available);
        PartyMaster.Instance.availableMercPrefabs.Add(MercPrefabs.Instance.EnumToPawnPrefab(mn)); //consider unifying with ChangeMercAssignment - perhaps switch(newAssignment)
        currentPlayerData.ChangeMercAssignment(mn, MercAssignment.Available, -1);

        //GoogleSheetMaster.Instance.LogPlayer(); //saves these changes
    }
    public void RemoveMercSheet(MercName mn) //specifically DOESNT remove any other members from lists (such as PartyMaster). Could do it if we also got a MercAssignment and switch on which list to remove from, but each time this is called - is specific and should manage its own removals individually
    {
        currentPlayerData.RemoveMercSheet(mn);
    }

    public List<MercName> SheetsAsNames()
    {
        List<MercName> toReturn = new List<MercName>();
        foreach (var item in currentPlayerData.mercSheets)
        {
            toReturn.Add(item.characterName);
        }

        return toReturn;
    }
    public MercSheet SheetByName(MercName mercName)
    {
        MercSheet toReturn = currentPlayerData.mercSheets.Where(x => x.characterName == mercName).SingleOrDefault();
        if (toReturn.characterName == mercName)
            return toReturn;
        else
            return null;
    }

    public void AddSiteCooldown(string levelSOName, DateTime dateTime)
    {
        if(!SiteCooldowns.ContainsKey(levelSOName))
        {
            SiteCooldowns.Add(levelSOName, dateTime);
        }
        else
        {
            SiteCooldowns[levelSOName] = dateTime;
        }
    }
    public void ClearSiteCooldown(string levelSOName)
    {
        if (!SiteCooldowns.ContainsKey(levelSOName))
        {
            SiteCooldowns.Add(levelSOName, null);
        }
        else
        {
            SiteCooldowns[levelSOName] = null;
        }
    }

    public void LogSquadDeparture(int squadIndex, LevelEnum level, DateTime tOfDeparture)
    {
        currentPlayerData.squadSitesAndTimesRemainning.Add(new SquadSiteAndTimeOfDeparture(squadIndex, level, tOfDeparture));
    }

    public void RemoveLoggedSquad()
    { 
    }

    public void RemoveLoggedSquad(int squadIndex)
    {
        tempSquadIndex = squadIndex;
       Invoke(nameof(RemoveLoggedSquadDelay), .1f);
    }
    int tempSquadIndex =-1;
    void RemoveLoggedSquadDelay()
    {
        //currentPlayerData.squadSitesAndTimesRemainning.Remove(currentPlayerData.squadSitesAndTimesRemainning.Where(x => x.squadIndex == tempSquadIndex).FirstOrDefault());
        SquadSiteAndTimeOfDeparture? temp = null;
        foreach (var item in currentPlayerData.squadSitesAndTimesRemainning)
        {
            if(item.squadIndex == tempSquadIndex)
            {
                temp = item;
                break;
            }
        }
        if (temp.HasValue)
            currentPlayerData.squadSitesAndTimesRemainning.Remove(temp.Value);
        else
            print("couldn't remove logged squad");
            

        tempSquadIndex = -1;
    }


    //Player data editing // NEW move every method here TBF this is the right way to do it
    public void AddToPsionNulMax(NulColour col, float amount)
    {
        currentPlayerData.psionSpectrum.IncreaseMaxValue(col, amount); 
    }

    public List<System.Object> GetLog
    {
        get => new List<System.Object> { currentPlayerData.playerName,
        currentPlayerData.victories,
        currentPlayerData.losses,
        currentPlayerData.numOfavailableMercs,
        currentPlayerData.cowardMercs,
        currentPlayerData.deadMercs,
        currentPlayerData.gold,
        currentPlayerData.totalMercLevel };
    }
}
