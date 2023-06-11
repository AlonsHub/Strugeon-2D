using System.Collections;
using System.Linq ;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.IO;

public class GoogleSheetMaster : MonoBehaviour
{
    static string spreadsheetID = "1VUnI_fzIOlRspR7lmM812egyi-kXYP0ltf7qUb7ufQw";
    static string path = "/StreamingAssets/credentials.json";
    static SheetsService sheetsService;

    [SerializeField]
    Sprite s;
    [SerializeField]
    bool doSheet;

    [SerializeField]
    AllItemSOs allItemsSO;

    public static bool DoSheet { get => Instance.doSheet; set => Instance.doSheet = value; }

    public static GoogleSheetMaster Instance;

    List<string> strings; //all A1:A35 first lines in the 

    string currentRangeName;
    int rowsPerIterration = 10;

    string itemCountCellCordinate = "Item_DataBase_Test2!K1";

    void Start()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpCredentials();

        if(DoSheet)
        LogPlayer(); // not sure we should

        //TEST
        //LogNewItem();
    }
    void SetUpCredentials() //this must also be done async  
    {
        string fullPath = Application.dataPath + path;

        Stream creds = File.Open(fullPath, FileMode.Open);

        ServiceAccountCredential serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(creds);

        sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = serviceAccountCredential });
    }
    
    public void PrintCell()
    {
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetID, "A1");

        var response = request.Execute();
        var values = response.Values;

        if (values != null && values.Count >= 0)
        {
            foreach (var row in values)
            {
                Debug.Log(row[0]);
            }
        }
        else
        {
            Debug.Log("No data");
        }
    }
    public List<string> GetFirstsOfRows(string range)
    {
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetID, range);

        var response = request.Execute(); //could not async?
        var values = response.Values;

        if (values != null && values.Count >= 0)
        {
            //return (List<string>)values;
            List<string> rowsFirstCells = new List<string>();
            foreach (var row in values)
            {
                rowsFirstCells.Add((string)row[0]);
            }
            return rowsFirstCells;
        }
        else
        {
            return null;
        }
    }
    public List<string[]> GetRows(string range)
    {
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetID, range);

        var response = request.Execute(); //could not async?
        var values = response.Values;

        if (values != null && values.Count >= 0)
        {
            //return (List<string>)values;
            List<string[]> rows = new List<string[]>();
            foreach (var row in values)
            {
                string[] ss = new string[row.Count];
                for (int i = 0; i < row.Count; i++)
                {
                    ss[i] = (string)row[i];
                }
                rows.Add(ss);
                //rows.Add((string[])row);
            }
            return rows;
        }
        else
        {
            return null;
        }
    }
     public List<string> GetRow(string range)
    {
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetID, range);

        var response = request.Execute(); //could not async?
        var values = response.Values;

        if (values != null && values.Count >= 0)
        {
            //return (List<string>)values;
            //List<string> rows = new List<string>();
            //foreach (var row in values)
            //{
            //    rows.Add(row);
            //}
            return values[0] as List<string>;
        }
        else
        {
            return null;
        }
    }


    private void OnApplicationQuit()
    {
        LogPlayer();
    }
    public void LogPlayer()
    {
        if (!doSheet)
            return;

        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
        var objectList = new List<System.Object>(); //fill object list with things!

        //if(strings==null)
        //strings = GetRows("A1:A35"); //currently limited to 35 different players. Disregards any new name after the 35th, but will continue to update any of the first 35

        if(currentRangeName == null)
        {
            if(!FindRangeForName(PlayerDataMaster.Instance.currentPlayerData.playerName))
            {
                Debug.LogError("somehow you don't have an entry in the scoreboard, so I'll make one for you - no problems! - you being: " + PlayerDataMaster.Instance.currentPlayerData.playerName);
                LogNewPlayer();
            }
            else
            {
                //all is good, assume currentRangeName is correct
            }
        }

        ///Solution to player limit
        ///loop that collects GetRows of a set size from A##:A[##+sizePerLoopItteration] and checks if the last memeber has any value
        ///if not, the loop ends
        ///if it does hold value, the loop goes again, collecting another [sizePerLoopItteration] of users, checking again if the last one holds any value

        //if (strings !=null && strings.Count >0 && strings.Contains(PlayerDataMaster.Instance.currentPlayerData.playerName)) 
        //{
        //    //True - the player is already logged:
        //    int i = strings.IndexOf(PlayerDataMaster.Instance.currentPlayerData.playerName);

        //    string newRange = "A" + (i+1);
            //objectList = new List<System.Object>() { PlayerDataMaster.Instance.currentPlayerData.playerName, PlayerDataMaster.Instance.currentPlayerData.lostMercs, PlayerDataMaster.Instance.currentPlayerData.numOfavailableMercs, PlayerDataMaster.Instance.currentPlayerData.gold};
            valueRange.Values = new List<IList<object>> { PlayerDataMaster.Instance.GetLog};

            var updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetID, currentRangeName);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            //var updateResponse = updateRequest.Execute();
            var updateResponse = updateRequest.ExecuteAsync();
        //}
        //else
        //{
        //    //objectList = new List<System.Object>() { PlayerDataMaster.Instance.currentPlayerData.playerName, PlayerDataMaster.Instance.currentPlayerData.deadMercs, PlayerDataMaster.Instance.currentPlayerData.numOfavailableMercs, PlayerDataMaster.Instance.currentPlayerData.gold };

        //    valueRange.Values = new List<IList<object>> { PlayerDataMaster.Instance.GetLog };
        //    //valueRange.Values = new List<IList<object>> { objectList };

        //    var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadsheetID, "A2"); //just adds under A
        //    appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        //    //var appendResponse = appendRequest.Execute();
        //    var appendResponse = appendRequest.ExecuteAsync();
        //}
    }
    public void LogNewPlayer()
    {
        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
        var objectList = new List<System.Object>(); //fill object list with things!


        valueRange.Values = new List<IList<object>> { PlayerDataMaster.Instance.GetLog };
        //valueRange.Values = new List<IList<object>> { objectList };

        var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadsheetID, "A2"); //just adds under A
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        //var appendResponse = appendRequest.Execute();
        var appendResponse = appendRequest.ExecuteAsync();

    }
    
    [ContextMenu("Log all existing items")]
    public void LogAllExistingItems()
    {
        foreach (var item in allItemsSO.GetAllItemSOList)
        {
            LogNewItem(item.magicItem);
        }
        //assuming all items are legit
        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
        var objectList = new List<System.Object>(); //fill object list with things!


        valueRange.Values = new List<IList<object>> { new List<object> { allItemsSO.GetAllItemSOList.Count } };
        //valueRange.Values = new List<IList<object>> { objectList };

        var updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetID, itemCountCellCordinate); //just adds under A
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        //var appendResponse = appendRequest.Execute();
        var response = updateRequest.ExecuteAsync();
    }

    [ContextMenu("Read all items")]
    public void ReadItems()
    {
        List<string> rows = GetFirstsOfRows(itemCountCellCordinate);
        int itemCount;
        print(rows[0]);
        if(int.TryParse(rows[0], out itemCount))
        {
            print($"parsed well into {itemCount}");
            rows = GetFirstsOfRows($"Item_DataBase_Test2!A2:D{itemCount+1}"); //+1 since the first row is header and not items

            MagicItem newItem = ParseItem(rows);
            MagicItemSO tempSO = ScriptableObject.CreateInstance<MagicItemSO>();
            //Dont fetch sprite yet!
            ItemDatabase.Instance.AddItem(newItem.magicItemName, tempSO);

            ////NOT YET TO ALL
            //foreach (var item in rows)
            //{
            //    //Read as:
            //    //magicItemName, fittingSlotType, myBenefit.BenefitStatName(), myBenefit.Value(), classes, pillProfile.AsStringData(),  goldValue.ToString(), spriteName };

            //}
        }
    }
    [ContextMenu("Read a new item")]
    public void ReadANewItem()
    {
        List<string> rows = GetFirstsOfRows(itemCountCellCordinate);
        var item = new List<string[]>();
        int itemCount;
        print(rows[0]);
        if(int.TryParse(rows[0], out itemCount))
        {
            print($"parsed well into {itemCount}");
            item = GetRows($"Item_DataBase_Test2!A2:H{itemCount+2}"); //+1 since the first row is header and not items
            //print(item[0]);
            //print(item[1][0]);

            MagicItem newItem = ParseItem(item[0]);
            MagicItemSO tempSO = ScriptableObject.CreateInstance<MagicItemSO>();
            tempSO.magicItem = newItem;
            //Dont fetch sprite yet!
            ItemDatabase.Instance.AddItem(newItem.magicItemName, tempSO);
            ItemDatabase.Instance.insoilMystica = tempSO;
            tempSO.CallFetch();
        }
    }

    MagicItem ParseItem(List<string> rows)
    {
        StatBenefit sb = new StatBenefit((StatToBenefit)int.Parse(rows[2]), int.Parse(rows[3]));
        List<MercClass> rClasses = new List<MercClass>();
        string[] seperated = rows[4].Split('_');
        foreach (var item in seperated)
        {
            item.Replace("_", "");
            
            switch (item)
            {
                case "Fighter":
                    rClasses.Add(MercClass.Fighter);
                    break;
                case "Rogue":
                    rClasses.Add(MercClass.Rogue);
                    break;
                case "Mage":
                    rClasses.Add(MercClass.Mage);
                    break;
                case "Priest":
                    rClasses.Add(MercClass.Priest);
                    break;
            }
        }
        seperated = rows[5].Split('_');
        float[] flots = new float[seperated.Length];
        for (int i = 0; i < seperated.Length; i++)
        {
            seperated[i].Replace("_", "");
            flots[i] = float.Parse(seperated[i]);
        }


        return new MagicItem(rows[0], (EquipSlotType)int.Parse(rows[1]), sb, rClasses, flots, int.Parse(rows[6]), rows[7]);
    }
    MagicItem ParseItem(string[] rows)
    {
        StatBenefit sb = new StatBenefit((StatToBenefit)int.Parse(rows[2]), int.Parse(rows[3]));
        List<MercClass> rClasses = new List<MercClass>();
        string[] seperated = rows[4].Split('_');
        foreach (var item in seperated)
        {
            item.Replace("_", "");
            
            switch (item)
            {
                case "Fighter":
                    rClasses.Add(MercClass.Fighter);
                    break;
                case "Rogue":
                    rClasses.Add(MercClass.Rogue);
                    break;
                case "Mage":
                    rClasses.Add(MercClass.Mage);
                    break;
                case "Priest":
                    rClasses.Add(MercClass.Priest);
                    break;
            }
        }
        seperated = rows[5].Split('_');
        float[] flots = new float[seperated.Length];
        for (int i = 0; i < seperated.Length-1; i++)
        {
            seperated[i].Replace("_", "");
            flots[i] = float.Parse(seperated[i]);
        }


        return new MagicItem(rows[0], (EquipSlotType)int.Parse(rows[1]), sb, rClasses, flots, int.Parse(rows[6]), rows[7]);
    }

    //public void LogRandomItems()
    //{
    //        LogNewItem(DifficultyTranslator.Instance.DifficultyToSingleReward(LairDifficulty.Easy));
    //}

    public void LogNewItem(MagicItem magicItem)
    {
        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
        var objectList = new List<System.Object>(); //fill object list with things!

        valueRange.Values = new List<IList<object>> { magicItem.DataAsListOfObjects() };

        var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadsheetID, "Item_DataBase_Test2!A2"); 
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        
        //var appendResponse = appendRequest.ExecuteAsync();
        var appendResponse = appendRequest.Execute();
    }
    //public void LogAllNewItemsBatch()
    //{
       

    //    //valueRange.Values = new List<IList<object>> { new List<System.Object> { magicItem.magicItemName,magicItem.pillProfile.AsStringData(), magicItem.goldValue, magicItem.spriteName} };

    //    var batchRequest = new BatchUpdateValuesRequest();
    //    batchRequest.Data = new List<ValueRange>();
    //    //var values =
    //    //batchRequest.Data;

    //    foreach (var magicItem in allItemsSO.GetAllItemSOList)
    //    {
    //        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
    //        var objectList = new List<System.Object>(); //fill object list with things!

    //        var itemStrings = magicItem.magicItem.AsStringsData();
    //        valueRange.Values = new List<IList<object>> { new List<System.Object> {magicItem.magicItem.AsStringsData()} };
    //        valueRange.Range = "Item_DataBase_Test2!A2";
    //        batchRequest.Data.Add(valueRange);

    //        //var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadsheetID, "Item_DataBase_Test2!A2");
    //        //appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
    //    }
    //    batchRequest.ValueInputOption = "USER_ENTERED";
    //    //batchRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED.ToString();
    //    var request = sheetsService.Spreadsheets.Values.BatchUpdate(batchRequest, spreadsheetID);
    //    var respone = request.ExecuteAsync();
    //    //var respone = request.Execute();

    //    print(respone.Result);
    //    //var appendResponse = appendRequest.ExecuteAsync();
    //}

    bool FindRangeForName(string nameToSearch) //sets currentRangeName
    {
        int start = 2;
        int end = start + rowsPerIterration - 1; //for a total of A2:A11(2+10-1) which is 10
        bool keepGoing = true;

        int runningtotal= 0;

        while (keepGoing)
        {
            List<string> s = GetFirstsOfRows("A" + start + ":A" + end);
            if (s == null) //string.NullOrEmpty()
            {
                currentRangeName = null;
                return false; //couldn't find name
            }
            
            if (s.Count > 0 )
            {
                if (s.Contains(PlayerDataMaster.Instance.currentPlayerData.playerName))
                {
                    int i = s.IndexOf(PlayerDataMaster.Instance.currentPlayerData.playerName);

                    currentRangeName = "A" + (runningtotal+ i + 2); //names start at line 2, i starts at 0
                    keepGoing = false; //redundant
                    break;
                }
                //if(s[s.Count-1] == "")
                if (s.Count() < rowsPerIterration)
                {
                    //keepGoing = false; //redundant
                    currentRangeName = null;
                    return false;
                    //currentRangeName = "A"+(runningtotal+1).ToString();
                    //return false;
                }
            }

            runningtotal += s.Count;
            start = end + 1;
            end = start + rowsPerIterration -1; //for a total of A12:A21(12+10-1) which is 10

        }
        return true;
    }
}
