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

    public static GoogleSheetMaster Instance;

    List<string> strings; //all A1:A35 first lines in the 

    string currentRangeName;
    int rowsPerIterration = 10;
    void Start()
    {
        Debug.LogError("Google Sheets Master perfoms start");
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpCredentials();

        //strings = GetRows("A1:A35");
        if(!FindRangeForName(PlayerDataMaster.Instance.currentPlayerData.playerName))
        {
            //currentRangeName is already set to null

        }
        LogPlayer(); // not sure we should
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
    public List<string> GetRows(string range)
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

    private void OnApplicationQuit()
    {
        LogPlayer();
    }
    public void LogPlayer()
    {
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

    bool FindRangeForName(string nameToSearch) //sets currentRangeName
    {
        int start = 2;
        int end = start + rowsPerIterration - 1; //for a total of A2:A11(2+10-1) which is 10
        bool keepGoing = true;

        int runningtotal= 0;

        while (keepGoing)
        {
            List<string> s = GetRows("A" + start + ":A" + end);
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
